using Telosatis.Application.Errors;

namespace Telosatis.Application.Features.Tokens;

public static class Errors
{
    public static readonly OperationError TokenWithGivenNameAlreadyExists = new("AT0001", "Token with the given name already exists.");
    public static readonly OperationError TokenWithRootAdminRoleCanNotBeCreated = new("AT0002", "Token with the root admin role cannot be created.");
    public static readonly OperationError TokenWithRootAdminRoleAlreadyExists = new("AT0003", "Token with the root admin role already exists.");
    public static readonly OperationError TokenWithRootAdminRoleCanNotBeDeleted = new("AT0004", "Token with the root admin role cannot be deleted.");
    
    public static readonly NotFoundError TokenNotFound = new("AT0005", "Token was not found.");
    
    public static readonly ValidationError TokenNameCanNotBeEmpty = new("AT0006", "Token name cannot be null or empty.");
    public static readonly ValidationError TokenRoleTypeIsNotSupported = new("AT0007", "Token role type is not supported.");
    public static readonly ValidationError TokenExpiresAtCanNotBeEmpty = new("AT0008", "Token expiration date cannot be empty.");
    public static readonly ValidationError TokenIdCanNotBeEmpty = new("AT0009", "Token ID cannot be null or empty.");
    public static readonly ValidationError ListTokensOrderKeyNotSupported = new("AT0010", "Token listing order key is not supported.");
}