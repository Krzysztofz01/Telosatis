using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telosatis.Application.Pipelines;
using Telosatis.Application.Services;
using Telosatis.Application.Services.Abstractions;

namespace Telosatis.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddTelosatisApplication(this IServiceCollection services)
    {
        services.TryAddSingleton<TimeProvider>(TimeProvider.System);
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });
        
        return services;
    }
}