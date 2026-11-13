using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Telosatis.Api.Contracts;
using Telosatis.Api.ExceptionHandlers;

namespace Telosatis.Api.Configuration;

internal static class GlobalErrorHandlingConfiguration
{
    public static IServiceCollection AddGlobalErrorHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        
        services.PostConfigure<ApiBehaviorOptions>(options =>
        {
            options.SuppressMapClientErrors = true;
            
            options.InvalidModelStateResponseFactory = _ => new BadRequestObjectResult(new ApiResponse<object>(
                (int)HttpStatusCode.BadRequest,
                null,
                Errors.MalformedRequestBody));
        });
 
        return services;
    }

    public static WebApplication UseGlobalErrorHandling(this WebApplication app)
    {
        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            ExceptionHandler = context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Task.CompletedTask;
            }
        });

        return app;
    }
}
