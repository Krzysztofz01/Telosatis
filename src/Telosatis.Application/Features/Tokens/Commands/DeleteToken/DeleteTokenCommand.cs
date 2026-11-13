using Telosatis.Application.Abstractions;
using Telosatis.Common;

namespace Telosatis.Application.Features.Tokens.Commands.DeleteToken;

public sealed record DeleteTokenCommand(
    Guid TokenId) : ICommand<Result<Guid>>;