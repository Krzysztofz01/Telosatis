using Telosatis.Common;

namespace Telosatis.Domain.Token;

public enum TokenRoleType
{
    RootAdmin = 1,
    Admin = 2,
}

public sealed class TokenRole : ValueObject<TokenRole>
{
    public TokenRoleType Value { get; private set; }

    private TokenRole(TokenRoleType value)
    {
        Value = value;
    }

    public static Result<TokenRole> FromTokenRoleType(TokenRoleType type)
    {
        if (type == default || !Enum.IsDefined(type))
            return Errors.TokenRoleNotSupported;

        return new TokenRole(type);
    }
}