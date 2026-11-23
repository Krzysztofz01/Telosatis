using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Telosatis.Api.Contracts;
using Telosatis.Api.RateLimiting;
using Telosatis.Application.Features.Authentication.Commands.Authenticate;
using Telosatis.Application.Features.Authentication.Commands.Reauthenticate;
using Telosatis.Application.Features.Authentication.Contracts;

namespace Telosatis.Api.Controllers;

[Route("api/auth")]
public sealed class AuthController(ISender sender) : BaseController
{
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitingPolicies.Authentication)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<AuthenticatedDto>>> Authenticate(
        [FromBody] AuthenticateDto body, 
        CancellationToken cancellationToken)
    {
        var command = new AuthenticateCommand(body.Secret);
        
        var result = await sender.Send(command, cancellationToken);
        
        return result.IsSuccess 
            ? OkFromResult(result)
            : BadRequestFromError<AuthenticatedDto>(result.Error);
    }
    
    [HttpPost("re")]
    public async Task<ActionResult<ApiResponse<AuthenticatedDto>>> Reauthenticate(
        CancellationToken cancellationToken)
    {
        var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var tokenId = nameIdentifierClaim is not null
            ? Guid.Parse(nameIdentifierClaim.Value) 
            : Guid.Empty;
        
        var command = new ReauthenticateCommand(tokenId);
        
        var result = await sender.Send(command, cancellationToken);
        
        return result.IsSuccess
            ? OkFromResult(result)
            : BadRequestFromError<AuthenticatedDto>(result.Error);
    }
}