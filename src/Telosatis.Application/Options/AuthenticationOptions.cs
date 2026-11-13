using Telosatis.Common;

namespace Telosatis.Application.Options;

public sealed class AuthenticationOptions : IValidableOptions
{
    public required string Key { get; init; }
    
    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(Key)) return false;

        return true;
    }
}