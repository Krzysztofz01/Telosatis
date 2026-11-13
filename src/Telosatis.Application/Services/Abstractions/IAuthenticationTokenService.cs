using Telosatis.Domain.Token;

namespace Telosatis.Application.Services.Abstractions;

public interface IAuthenticationTokenService
{
    Task<AuthenticationToken> CreateAuthenticationToken(Token token, CancellationToken cancellationToken = default);
}

public sealed record AuthenticationToken(
    string Value,
    DateTimeOffset ExpiresAt);