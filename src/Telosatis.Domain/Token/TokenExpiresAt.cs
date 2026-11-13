using Telosatis.Common;

namespace Telosatis.Domain.Token;

public sealed class TokenExpiresAt : ValueObject<TokenExpiresAt>
{
    public DateTimeOffset? Value { get; private set; }

    private TokenExpiresAt(DateTimeOffset? value)
    {
        Value = value;
    }

    public static Result<TokenExpiresAt> WithoutExpiration => new TokenExpiresAt(null);
    
    public static Result<TokenExpiresAt> FromDateTimeOffset(DateTimeOffset expiresAt, DateTimeOffset now)
    {
        if (now >= expiresAt)
            return Errors.TokenExpiresAtOutOfRange;

        return new TokenExpiresAt(expiresAt);
    }
}