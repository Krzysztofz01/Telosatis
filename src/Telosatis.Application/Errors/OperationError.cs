using Telosatis.Common;

namespace Telosatis.Application.Errors;

public record OperationError(string Id, string Description) : Error(Id, Description);