using Telosatis.Application.Errors;

namespace Telosatis.Application.Features.Authentication;

public static class Errors
{
    public static readonly AuthorizationError FailedToHashSecret = new("AA0001", "Failed to hash the secret.");
    public static readonly AuthorizationError MatchingSecretHashNotFound = new("AA0002", "No matching secret hash was found.");
    public static readonly AuthorizationError FailedToCreateAuthenticationToken = new("AA0003", "Failed to create the authentication token.");
    public static readonly AuthorizationError TokenExpired = new("AA0004", "Token has expired.");
    public static readonly AuthorizationError MatchingTokenNotFound = new("AA0005", "No matching token was found.");
    
    public static readonly ValidationError SecretCanNotBeEmpty = new("AA0006", "Secret cannot be null or empty.");
    public static readonly ValidationError TokenIdCanNotBeEmpty = new("AA0007", "Token ID cannot be null or empty.");
}