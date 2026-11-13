using Telosatis.Application.Errors;

namespace Telosatis.Application.Features.Authentication;

// FIXME: Implement
public static class Errors
{
    public static readonly AuthorizationError FailedToHashSecret = new("", "");
    public static readonly AuthorizationError MatchingSecretHashNotFound = new("", "");
    public static readonly AuthorizationError FailedToCreateAuthenticationToken = new("", "");
    public static readonly AuthorizationError TokenExpired = new("", "");
    public static readonly AuthorizationError MatchingTokenNotFound = new("", "");
    
    public static readonly ValidationError SecretCanNotBeEmpty = new("", "");
    public static readonly ValidationError TokenIdCanNotBeEmpty = new("", "");
}