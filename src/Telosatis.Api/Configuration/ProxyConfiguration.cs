using System.Net;
using Microsoft.AspNetCore.HttpOverrides;
using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;

namespace Telosatis.Api.Configuration;

internal static class ProxyConfiguration
{
    public static IServiceCollection AddReverseProxySupport(
        this IServiceCollection services,
        IEnumerable<string>? trustedProxies,
        IEnumerable<string>? trustedNetworks)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

            if (trustedProxies is not null)
            {
                options.KnownProxies.Clear();
                
                foreach (var ip in trustedProxies)
                    options.KnownProxies.Add(IPAddress.Parse(ip));
            }

            if (trustedNetworks is not null)
            {
                options.KnownNetworks.Clear();
                
                foreach (var cidr in trustedNetworks)
                    options.KnownNetworks.Add(IPNetwork.Parse(cidr));
            }
        });
        
        return services;
    }

    public static WebApplication UseReverseProxySupport(this WebApplication app)
    {
        app.UseForwardedHeaders();

        return app;
    }
}