using Telosatis.Application.Errors;

namespace Telosatis.Api.Contracts;

public static class Errors
{
    public static readonly AuthorizationError AuthenticationFailed = new("API0001", "Authentication failed.");
    
    public static readonly Error InternalServerError = new("API0002", "An internal server error occurred.");
    public static readonly Error TooManyRequests = new("API0003", "Too many requests. Please try again later.");
    public static readonly Error MalformedRequestBody = new("API0004", "Request body is malformed or could not be parsed.");
}
