using Telosatis.Common;

namespace Telosatis.Application.Errors;

public record AuthorizationError(string Id, string Description) : Error(Id, Description);