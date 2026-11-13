using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Telosatis.Application.Options;
using Telosatis.Application.Services.Abstractions;
using Telosatis.Infrastructure.Extensions;

namespace Telosatis.Infrastructure.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddJsonWebTokenAuth(this IServiceCollection services, IConfiguration configuration, string url)
    {
        var options = services.AddAndResolveOptions<AuthenticationOptions>(configuration);

        var keyBytes = Encoding.UTF8.GetBytes(options.Key);
        var key = new SymmetricSecurityKey(keyBytes);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = url,
                    ValidateAudience = true,
                    ValidAudience = url,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role,
                };
            });

        services.AddAuthorization();
        
        services.TryAddScoped<IAuthenticationTokenService, AuthenticationTokenService>();
        
        return services;
    }

    public static WebApplication UseJsonWebTokenAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}