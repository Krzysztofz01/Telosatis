using Telosatis.Common;

namespace Telosatis.Domain.Token;

public static class Errors
{
    public static readonly Error TokenNameCanNotBeEmpty = new("DT0001", "Token name cannot be null or empty.");
    public static readonly Error TimeProviderCanNotBeNull = new("DT0002", "Time provider cannot be null.");
    public static readonly Error TokenExpiresAtOutOfRange = new("DT0003", "Token expiration date is out of the allowed range.");
    public static readonly Error TokenNameLengthExceeded = new("DT0004", "Token name exceeds the maximum allowed length.");
    public static readonly Error TokenSecretHashLengthMismatch = new("DT0005", "Token secret hash length does not match the expected length.");
    public static readonly Error TokenSecretFormatIsInvalid = new("DT0006", "Token secret format is not valid.");
    public static readonly Error TokenRoleNotSupported = new("DT0007", "Token role is not supported.");
    public static readonly Error TokenWithRootAdminRoleMustNotExpire = new("DT0008", "Token with the root admin role must not have an expiration date.");
}