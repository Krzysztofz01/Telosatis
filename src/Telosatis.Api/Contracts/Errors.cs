namespace Telosatis.Api.Contracts;

public static class Errors
{
    public static readonly Error InternalServerError = new("", "");
    public static readonly Error TooManyRequests = new("", "");
    public static readonly Error MalformedRequestBody = new("", "");
}