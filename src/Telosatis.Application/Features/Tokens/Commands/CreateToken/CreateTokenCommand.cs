using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Tokens.Contracts;
using Telosatis.Common;
using Telosatis.Domain.Token;

namespace Telosatis.Application.Features.Tokens.Commands.CreateToken;

public sealed record CreateTokenCommand(
    string Name,
    TokenRoleType RoleType,
    DateTimeOffset ExpiresAt) : ICommand<Result<TokenCreatedDto>>;