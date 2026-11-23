using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telosatis.Api.Contracts;
using Telosatis.Application.Errors;
using Telosatis.Application.Features.Files.Contracts;
using Telosatis.Common;
using Error = Telosatis.Common.Error;

namespace Telosatis.Api.Controllers;

[ApiController]
[Authorize]
public abstract class BaseController : ControllerBase
{
    [NonAction]
    protected ActionResult<ApiResponse<T>> OkFromResult<T>(Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result.Value);
        
        var response = new ApiResponse<T>(
            (int)HttpStatusCode.OK,
            result.Value,
            null);

        return base.Ok(response);
    }

    [NonAction]
    protected FileResult FileFromResult(Result<FileDto> result)
    {
        ArgumentNullException.ThrowIfNull(result.Value);
        
        var file = result.Value;
        HttpContext.Response.RegisterForDisposeAsync(file);
        
        return File(
            file.Content,
            file.Info.ContentType,
            file.Info.Name,
            true);
    }

    [NonAction]
    protected IActionResult BadRequestFromError(Error error)
    {
        var response = new ApiResponse<object?>(
            (int)HttpStatusCode.BadRequest,
            null,
            new Contracts.Error(
                error.Id,
                error.Description));

        return base.BadRequest(response);
    }
    
    [NonAction]
    protected ActionResult<ApiResponse<T>> BadRequestFromError<T>(Error error)
    {
        var response = new ApiResponse<T>(
            (int)HttpStatusCode.BadRequest,
            default,
            new Contracts.Error(
                error.Id,
                error.Description));

        return base.BadRequest(response);
    }

    [NonAction]
    protected IActionResult NotFoundFromError(NotFoundError error)
    {
        var response = new ApiResponse<object?>(
            (int)HttpStatusCode.NotFound,
            null,
            new Contracts.Error(
                error.Id,
                error.Description));

        return base.NotFound(response);
    }
    
    [NonAction]
    protected ActionResult<ApiResponse<T>> NotFoundFromError<T>(NotFoundError error)
    {
        var response = new ApiResponse<T>(
            (int)HttpStatusCode.NotFound,
            default,
            new Contracts.Error(
                error.Id,
                error.Description));

        return base.NotFound(response);
    }
}

