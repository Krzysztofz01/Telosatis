using System.Globalization;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Telosatis.Api.Logging;

namespace Telosatis.Api.Configuration;

internal static class LoggingConfiguration
{
    private const string OutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}";
 
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        // TODO: Make the log levels configurable
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: OutputTemplate,
                formatProvider: CultureInfo.InvariantCulture,
                theme: Console.IsOutputRedirected ? ConsoleTheme.None : AnsiConsoleTheme.Code)
            .CreateLogger();
 
        builder.Services.AddSerilog();
        
        return builder;
    }

    public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();
     
        return app;
    }
}
