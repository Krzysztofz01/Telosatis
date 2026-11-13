using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Authentication.Contracts;
using Telosatis.Application.Services.Abstractions;
using Telosatis.Common;
using Telosatis.Domain.Token;

namespace Telosatis.Application.Features.Authentication.Commands.Authenticate;

internal sealed class AuthenticateCommandHandler(
    IApplicationDbContext dbContext,
    ILogger<AuthenticateCommandHandler> logger,
    IAuthenticationTokenService authenticationTokenService,
    TimeProvider timeProvider)
    : ICommandHandler<AuthenticateCommand, Result<AuthenticatedDto>>
{
    public async Task<Result<AuthenticatedDto>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        
        var secretHashResult = Token.GetSecretHash(request.Secret);
        if (secretHashResult.IsFailure)
        {
            logger.LogInformation("Authentication failed due to secret hashing failure: {@Error}", secretHashResult.Error);
            return Errors.FailedToHashSecret;
        }

        var secretHash = secretHashResult.Value;
        
        var token = await dbContext.Tokens.SingleOrDefaultAsync(x => x.SecretHash.Value == secretHash, cancellationToken);
        if (token is null)
        {
            logger.LogInformation("Authentication failed due to lack of matching hash secret");
            return Errors.MatchingSecretHashNotFound;
        }

        if (now > token.ExpiresAt.Value)
        {
            logger.LogInformation("Authentication for Token: {TokenId} failed due to token expiration", token.Id);
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
            logger.LogInformation(ex, "Authentication for Token: {TokenId} failed due to auth token creation failure", token.Id);
            return Errors.FailedToCreateAuthenticationToken;
        }
    }
}