using Telosatis.Common;

namespace Telosatis.Domain.Token;

// FIXME: Implement
public static class Errors
{
    public static Error TokenNameCanNotBeEmpty { get; set; }
    public static Error TimeProviderCanNotBeNull { get; set; }
    public static Error TokenExpiresAtOutOfRange { get; set; }
    public static Result<TokenName> TokenNameLengthExceeded { get; set; }
    public static Result<TokenSecret> TokenSecretHashLengthMissmatch { get; set; }
    public static Result<TokenSecret> TokenSecretFormatIsInvalid { get; set; }
    public static Result<TokenRole> TokenRoleNotSupported { get; set; }
}