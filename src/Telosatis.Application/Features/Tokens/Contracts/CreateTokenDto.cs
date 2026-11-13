using Telosatis.Domain.Token;

namespace Telosatis.Application.Features.Tokens.Contracts;

public sealed record CreateTokenDto(
    string Name,
    TokenRoleType RoleType,
    DateTimeOffset ExpiresAt);
