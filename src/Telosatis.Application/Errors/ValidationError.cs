using Telosatis.Common;

namespace Telosatis.Application.Errors;

public record ValidationError(string Id, string Description) : Error(Id, Description);