using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Authentication.Contracts;
using Telosatis.Application.Services.Abstractions;
using Telosatis.Common;

namespace Telosatis.Application.Features.Authentication.Commands.Reauthenticate;

internal sealed class ReauthenticateCommandHandler(
    IApplicationDbContext dbContext,
    ILogger<ReauthenticateCommandHandler> logger,
    IAuthenticationTokenService authenticationTokenService,
    TimeProvider timeProvider)

    : ICommandHandler<ReauthenticateCommand, Result<AuthenticatedDto>>
{
    public async Task<Result<AuthenticatedDto>> Handle(ReauthenticateCommand request, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        
        var token = await dbContext.Tokens.SingleOrDefaultAsync(x => x.Id == request.TokenId, cancellationToken);
        if (token is null)
        {
            logger.LogInformation("Reauthentication failed due to lack of matching token");
            return Errors.MatchingTokenNotFound;
        }

        if (now > token.ExpiresAt.Value)
        {
            logger.LogInformation("Reauthentication for Token: {TokenId} failed due to token expiration", token.Id);
            return Errors.TokenExpired;
        }
        
        try
        {
            var authenticationToken = await authenticationTokenService.CreateAuthenticationToken(token, cancellationToken);

            return new AuthenticatedDto(
                authenticationToken.Value,
                authenticationToken.ExpiresAt);
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex, "Reauthentication for Token: {TokenId} failed due to auth token creation failure", token.Id);
            return Errors.FailedToCreateAuthenticationToken;
        }
    }
}