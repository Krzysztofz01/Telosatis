using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading.RateLimiting;
using Telosatis.Api.Contracts;
using Telosatis.Api.RateLimiting;

namespace Telosatis.Api.Configuration;

internal static class RateLimitingConfiguration
{
    public static IServiceCollection AddPolicyBasedRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (context, _) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter =
                        ((int)Math.Ceiling(retryAfter.TotalSeconds)).ToString(CultureInfo.InvariantCulture);
                }
 
                var response = new ApiResponse<object>(
                    (int)HttpStatusCode.TooManyRequests,
                    null,
                    Errors.TooManyRequests);

                context.HttpContext.Response.StatusCode = response.StatusCode;
                await context.HttpContext.Response.WriteAsJsonAsync(response, CancellationToken.None);
            };

            options.AddPolicy(RateLimitingPolicies.Authentication, http =>
                RateLimitPartition.GetSlidingWindowLimiter(GetClientKey(RateLimitingPolicies.Authentication, http), _ => new()
                {
                   PermitLimit = RateLimitingSettings.AuthPermitLimit,
                   Window = RateLimitingSettings.AuthWindow,
                   SegmentsPerWindow = RateLimitingSettings.AuthSegments,
                   QueueLimit = 0,
                   AutoReplenishment = true,
                }));

            options.AddPolicy(RateLimitingPolicies.Onboarding, http =>
            {
                if (http.User.Identity?.IsAuthenticated == true)
                    return RateLimitPartition.GetNoLimiter("authenticated");
                
                return RateLimitPartition.GetSlidingWindowLimiter(GetClientKey(RateLimitingPolicies.Onboarding, http), _ => new()
                {
                    PermitLimit = RateLimitingSettings.OnboardingPermitLimit,
                    Window = RateLimitingSettings.OnboardingWindow,
                    SegmentsPerWindow = RateLimitingSettings.OnboardingSegments,
                    QueueLimit = 0,
                    AutoReplenishment = true,
                });
            });
        });

        return services;
    }

    private static string GetClientKey(string policy, HttpContext httpContext) =>
        $"{policy}:{ResolveIpAddress(httpContext)}";
    
    private static string ResolveIpAddress(HttpContext httpContext)
    {
        var ip = httpContext.Connection.RemoteIpAddress;
        if (ip is null) return string.Empty;

        if (ip.IsIPv4MappedToIPv6)
            ip = ip.MapToIPv4();

        if (ip.AddressFamily == AddressFamily.InterNetworkV6)
        {
            var addressBytes = ip.GetAddressBytes();
            Array.Clear(addressBytes, 8, 8);
            return $"{new IPAddress(addressBytes)}/64";
        }

        return ip.ToString();
    }
}