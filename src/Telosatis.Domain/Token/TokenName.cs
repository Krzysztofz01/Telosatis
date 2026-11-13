using Telosatis.Common;

namespace Telosatis.Domain.Token;

public sealed class TokenName : ValueObject<TokenName>
{
    public const int MaxLength = 128;
    
    public string Value { get; private set; }

    private TokenName(string value)
    {
        Value = value;
    }

    public static Result<TokenName> FromString(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.TokenNameCanNotBeEmpty;

        if (name.Length > MaxLength)
            return Errors.TokenNameLengthExceeded;
        
        return new TokenName(name);
    }
}