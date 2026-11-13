using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Telosatis.Api.Contracts;

namespace Telosatis.Api.ExceptionHandlers;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken _)
    {
        if (exception is OperationCanceledException && context.RequestAborted.IsCancellationRequested)
        {
            logger.LogInformation("Request {Method} {Path} was aborted by the client",
                context.Request.Method,
                context.Request.Path);
            
            context.Response.StatusCode = 499; 
            return true;
        }
 
        logger.LogError(exception, "Request {Method} {Path} failed with an unhandled exception: {ExceptionMessage}",
            context.Request.Method,
            context.Request.Path,
            exception.Message);
        
        var response = new ApiResponse<object>(
            (int)HttpStatusCode.InternalServerError,
            null,
            Errors.InternalServerError);
        
        context.Response.StatusCode = response.StatusCode;
        await context.Response.WriteAsJsonAsync(response, context.RequestAborted);
        return true;
    }
}
