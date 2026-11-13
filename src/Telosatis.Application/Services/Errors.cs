using Telosatis.Application.Errors;
using Telosatis.Common;

namespace Telosatis.Application.Services;

// FIXME: Implement
public static class Errors
{
    public static AuthorizationError InvalidSecretFormat { get; set; }
    public static Result TokenWithMatchingSecretNotFound { get; set; }
    public static Result TokenExpired { get; set; }
}