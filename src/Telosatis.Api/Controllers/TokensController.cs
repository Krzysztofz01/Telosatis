using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Telosatis.Api.Contracts;
using Telosatis.Api.RateLimiting;
using Telosatis.Application.Features.Tokens.Commands.CreateRootAdminToken;
using Telosatis.Application.Features.Tokens.Commands.CreateToken;
using Telosatis.Application.Features.Tokens.Commands.DeleteToken;
using Telosatis.Application.Features.Tokens.Contracts;
using Telosatis.Application.Features.Tokens.Queries.IsRootAdminCreated;
using Telosatis.Application.Features.Tokens.Queries.ListTokens;

namespace Telosatis.Api.Controllers;

[Route("api/tokens")]
public sealed class TokensController(ISender sender) : BaseController
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TokenCreatedDto>>> CreateToken(
        [FromBody] CreateTokenDto body, 
        CancellationToken cancellationToken)
    {
        var command = new CreateTokenCommand(
            body.Name,
            body.RoleType,
            body.ExpiresAt);

        var result = await sender.Send(command, cancellationToken);
        
        return result.IsSuccess 
            ? OkFromResult(result)
            : BadRequestFromError<TokenCreatedDto>(result.Error);
    }

    [HttpDelete("{tokenId:guid}")]
    public async Task<ActionResult<ApiResponse<Guid>>> DeleteToken(
        [FromRoute] Guid tokenId, 
        CancellationToken cancellationToken)
    {
        var command = new DeleteTokenCommand(tokenId);
        
        var result = await sender.Send(command, cancellationToken);
        
        return result.IsSuccess
            ? OkFromResult(result)
            : BadRequestFromError<Guid>(result.Error);
    }
    
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TokenDto>>>> ListTokens(
        [FromQuery(Name = "k")] ListTokensOrderKey? orderKey,
        [FromQuery(Name = "d")] bool? orderDescending,
        CancellationToken cancellationToken)
    {
        var query = new ListTokensQuery(
            orderKey,
            orderDescending);
        
        var result = await sender.Send(query, cancellationToken);
        
        return result.IsSuccess
            ? OkFromResult(result)
            : BadRequestFromError<IReadOnlyList<TokenDto>>(result.Error);
    }
    
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitingPolicies.Onboarding)]
    [HttpGet("onboarding")]
    public async Task<ActionResult<ApiResponse<TokenRootAdminCreatedDto>>> IsRootAdminCreated(
        CancellationToken cancellationToken)
    {
        var query = new IsRootAdminCreatedQuery();
        
        var result = await sender.Send(query, cancellationToken);
        
        return result.IsSuccess
            ? OkFromResult(result)
            : BadRequestFromError<TokenRootAdminCreatedDto>(result.Error);
    }
    
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitingPolicies.Onboarding)]
    [HttpPost("onboarding")]
    public async Task<ActionResult<ApiResponse<TokenCreatedDto>>> CreateRootAdminToken(
        CancellationToken cancellationToken)
    {
        var command = new CreateRootAdminTokenCommand();
        
        var result = await sender.Send(command, cancellationToken);
        
        return result.IsSuccess
            ? OkFromResult(result)
            : BadRequestFromError<TokenCreatedDto>(result.Error);
    }
}