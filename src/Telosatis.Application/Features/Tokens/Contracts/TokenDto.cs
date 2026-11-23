using Telosatis.Domain.Token;

namespace Telosatis.Application.Features.Tokens.Contracts;

public sealed record TokenDto(
    Guid Id,
    string Name,
    TokenRoleType RoleType,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ExpiresAt);