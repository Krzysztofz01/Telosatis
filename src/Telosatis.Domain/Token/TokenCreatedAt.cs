using Telosatis.Common;

namespace Telosatis.Domain.Token;

public sealed class TokenCreatedAt : ValueObject<TokenCreatedAt>
{
    public DateTimeOffset Value { get; private set; }

    private TokenCreatedAt(DateTimeOffset value)
    {
        Value = value;
    }

    public static Result<TokenCreatedAt> FromDateTimeOffset(DateTimeOffset createdAt)
    {
        return new TokenCreatedAt(createdAt);
    }
}