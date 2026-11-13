using Telosatis.Common;

namespace Telosatis.Application.Errors;

public record NotFoundError(string Id, string Description) : Error(Id, Description);