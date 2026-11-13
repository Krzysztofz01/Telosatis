using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telosatis.Common;

namespace Telosatis.Infrastructure.Extensions;

public static class ValidableOptionsExtensions
{
    public static TOptions AddAndResolveOptions<TOptions>(
        this IServiceCollection @this,
        IConfiguration configuration,
        bool validate = true)
        where TOptions : class, IValidableOptions =>
        AddAndResolveOptionsInternal<TOptions>(@this, configuration, validate);

    public static TOptions AddAndResolveOptions<TOptions>(
        this IHostApplicationBuilder @this,
        bool validate = true)
        where TOptions : class, IValidableOptions =>
        AddAndResolveOptionsInternal<TOptions>(@this.Services, @this.Configuration, validate);
    
    private static TOptions AddAndResolveOptionsInternal<TOptions>(
        IServiceCollection services,
        IConfiguration configuration,
        bool validate)
        where TOptions : class, IValidableOptions
    {
        var sectionName = typeof(TOptions).Name;

        services.AddOptions<TOptions>()
            .BindConfiguration(sectionName)
            .Validate(x => x.IsValid(), $"Invalid {sectionName} configuration.")
            .ValidateOnStart(validate);

        var resolvedOptions = configuration
            .GetSection(sectionName)
            .Get<TOptions>();

        if (resolvedOptions is null)
            throw new OptionsValidationException(
                Options.DefaultName,
                typeof(TOptions),
                [$"Failed to access {sectionName} configuration."]);
        
        if (validate && !resolvedOptions.IsValid())
            throw new OptionsValidationException(
                Options.DefaultName,
                typeof(TOptions),
                [$"Invalid {sectionName} configuration."]);

        return resolvedOptions;
    }
}