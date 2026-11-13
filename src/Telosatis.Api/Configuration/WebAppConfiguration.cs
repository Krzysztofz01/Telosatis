using System.Text.RegularExpressions;

namespace Telosatis.Api.Configuration;

internal static class WebAppConfiguration
{
    private static readonly Regex HashedFileNameRegex = new(
        @"-[A-Z0-9]{8}\.[a-z0-9]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant,
        TimeSpan.FromSeconds(1));

    private const string WebAppEntrypoint = "index.html";
    
    public static WebApplication UseTelosatisWebApplication(this WebApplication app)
    {
        if (!IsWebAppAvailable(app))
            return app;

        app.UseStaticFiles(WebAppFileOptions);
        
        return app;
    }

    public static WebApplication MapTelosatisWebApplication(this WebApplication app)
    {
        app.MapFallback("/api/{**path}", () => Results.NotFound());
        
        if (IsWebAppAvailable(app))
            app.MapFallbackToFile(WebAppEntrypoint, WebAppFileOptions);
        
        return app;
    }

    private static readonly StaticFileOptions WebAppFileOptions = new()
    {
        OnPrepareResponse = ctx =>
        {
            var headers = ctx.Context.Response.Headers;
            headers.XContentTypeOptions = "nosniff";

            headers.CacheControl = HashedFileNameRegex.IsMatch(ctx.File.Name)
                ? "public, max-age=31536000, immutable"
                : "no-cache";
        }
    };
    
    private static bool IsWebAppAvailable(WebApplication app) =>
        app.Environment.WebRootFileProvider.GetFileInfo(WebAppEntrypoint).Exists;
}