using Telosatis.Application.Errors;

namespace Telosatis.Application.Features.Tokens;

// FIXME: Implement
public static class Errors
{
    public static readonly OperationError TokenWithGivenNameAlreadyExists = new("", "");
    public static readonly OperationError TokenWithRootAdminRoleCanNotBeCreated = new("", "");
    public static readonly OperationError TokenWithRootAdminRoleAlreadyExists = new("", "");
    public static readonly OperationError TokenWithRootAdminRoleCanNotBeDeleted = new("", "");
    
    public static readonly NotFoundError TokenNotFound = new("", "");
    
    public static readonly ValidationError TokenNameCanNotBeEmpty = new("", "");
    public static readonly ValidationError TokenRoleTypeIsNotSupported = new("", "");
    public static readonly ValidationError TokenExpiresAtCanNotBeEmpty = new("", "");
    public static readonly ValidationError TokeIdCanNotBeEmpty = new("", "");
    public static readonly ValidationError ListTokensOrderKeyNotSupported = new("", "");
}