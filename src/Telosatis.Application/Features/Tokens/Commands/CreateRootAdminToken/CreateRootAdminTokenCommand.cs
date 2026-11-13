using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Tokens.Contracts;
using Telosatis.Common;

namespace Telosatis.Application.Features.Tokens.Commands.CreateRootAdminToken;

public sealed record CreateRootAdminTokenCommand : ICommand<Result<TokenCreatedDto>>;