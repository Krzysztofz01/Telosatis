using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Tokens.Contracts;
using Telosatis.Common;

namespace Telosatis.Application.Features.Tokens.Queries.ListTokens;

public sealed record ListTokensQuery(
    ListTokensOrderKey? OrderKey,
    bool? OrderDescending) : IQuery<Result<IReadOnlyList<TokenDto>>>;

public enum ListTokensOrderKey 
{
    Id = 1,
    Name = 2,
    RoleType = 3,
    ExpiresAt = 4,
    CreatedAt = 5,
}
