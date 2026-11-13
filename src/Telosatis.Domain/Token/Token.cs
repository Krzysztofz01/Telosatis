using Telosatis.Common;

namespace Telosatis.Domain.Token;

public sealed class Token : Entity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Token()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    }
    
    private Token(
        TokenName name,
        TokenSecretHash secretHash,
        TokenRole role,
        TokenCreatedAt createdAt,
        TokenExpiresAt expiresAt)
    {
        Name = name;
        SecretHash = secretHash;
        Role = role;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
    }

    public TokenName Name { get; private set; }
    public TokenSecretHash SecretHash { get; private set; }
    public TokenRole Role { get; private set; }
    public TokenCreatedAt CreatedAt { get; private set; }
    public TokenExpiresAt ExpiresAt { get; private set; }

    public static Result<(Token, string)> Create(string name, TokenRoleType roleType, DateTimeOffset? expiresAt, TimeProvider timeProvider)
    {
        if (timeProvider is null)
            return Errors.TimeProviderCanNotBeNull;

        var now = timeProvider.GetUtcNow();
        
        var nameResult = TokenName.FromString(name);
        if (nameResult.IsFailure)
            return nameResult.Error;

        var roleResult = TokenRole.FromTokenRoleType(roleType);
        if (roleResult.IsFailure)
            return roleResult.Error;
        
        var createdAtResult = TokenCreatedAt.FromDateTimeOffset(now);
        if (createdAtResult.IsFailure)
            return createdAtResult.Error;
        
        var expiresAtResult = expiresAt.HasValue
            ? TokenExpiresAt.FromDateTimeOffset(expiresAt.Value, now)
            : TokenExpiresAt.WithoutExpiration;

        if (expiresAtResult.IsFailure)
            return expiresAtResult.Error;

        var tokenSecretResult = TokenSecretHash.Create(now);
        if (tokenSecretResult.IsFailure)
            return tokenSecretResult.Error;

        var token = new Token(
            nameResult.Value,
            tokenSecretResult.Value.SecretHash,
            roleResult.Value,
            createdAtResult.Value,
            expiresAtResult.Value);

        return (token, tokenSecretResult.Value.Secret);
    }

    public static Result<string> GetSecretHash(string secret)
    {
        var tokenSecretResult = TokenSecretHash.FromSecret(secret);
        if (tokenSecretResult.IsFailure)
            return tokenSecretResult.Error;

        return tokenSecretResult.Value.SecretHash.Value;
    }
}