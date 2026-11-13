using System.Diagnostics;
using System.Security.Claims;

namespace Telosatis.Api.Logging;

public sealed class RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
{
    private const bool LogApiRequestsOnly = true;
    
    private readonly ILogger _logger = loggerFactory.CreateLogger("HTTP");
 
    public async Task InvokeAsync(HttpContext context)
    {
        var start = Stopwatch.GetTimestamp();
 
        try
        {
            await next(context);
        }
        finally
        {
            var elapsedMs = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
            
            LogRequest(context, elapsedMs);
        }
    }

    private void LogRequest(HttpContext context, double elapsedMs)
    {
        if (LogApiRequestsOnly && !context.Request.Path.StartsWithSegments("/api"))
            return;

        var statusCode = context.Response.StatusCode;
        var level = statusCode >= 500
            ? LogLevel.Error
            : LogLevel.Information;

        if (!_logger.IsEnabled(level))
            return;
        
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "-";
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "-";
        
        _logger.Log(level,
            "{Method} {Path} responded {StatusCode} in {ElapsedMs:0.0} ms (ip {ClientIp}, user {UserId})",
            context.Request.Method,
            context.Request.PathBase + context.Request.Path,
            statusCode,
            elapsedMs,
            ipAddress,
            userId);
    }
}
