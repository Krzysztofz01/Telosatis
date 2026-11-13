using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telosatis.Api.Contracts;
using Telosatis.Application.Errors;
using Telosatis.Application.Features.Files.Contracts;
using Error = Telosatis.Common.Error;

namespace Telosatis.Api.Controllers;

[ApiController]
[Authorize]
public abstract class BaseController : ControllerBase
{
    [NonAction]
    protected ActionResult<ApiResponse<T>> Ok<T>(T value)
    {
        var response = new ApiResponse<T>(
            (int)HttpStatusCode.OK,
            value,
            null);

        return base.Ok(response);
    }

    [NonAction]
    protected FileResult File(FileDto file)
    {
        HttpContext.Response.RegisterForDisposeAsync(file);
        
        return File(
            file.Content,
            file.Info.ContentType,
            file.Info.Name,
            true);
    }
 
    [NonAction]
    protected ActionResult<ApiResponse<T>> BadRequest<T>(Error error)
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
    protected ActionResult<ApiResponse<T>> NotFound<T>(NotFoundError error)
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

