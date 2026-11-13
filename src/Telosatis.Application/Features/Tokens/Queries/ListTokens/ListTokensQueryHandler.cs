using Microsoft.EntityFrameworkCore;
using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Tokens.Contracts;
using Telosatis.Application.Services;
using Telosatis.Application.Services.Abstractions;
using Telosatis.Common;

namespace Telosatis.Application.Features.Tokens.Queries.ListTokens;

internal sealed class ListTokensQueryHandler(
    IApplicationDbContext dbContext) : IQueryHandler<ListTokensQuery, Result<IReadOnlyList<TokenDto>>>
{
    public async Task<Result<IReadOnlyList<TokenDto>>> Handle(ListTokensQuery request, CancellationToken cancellationToken)
    {
        var orderKey = request.OrderKey ?? ListTokensOrderKey.Id;
        var orderDescending = request.OrderDescending ?? false;

        var query = dbContext.Tokens
            .AsQueryable();
        
        query = orderKey switch
        {
            ListTokensOrderKey.Id => orderDescending
                ? query.OrderByDescending(x => x.Id)
                : query.OrderBy(x => x.Id),
            ListTokensOrderKey.Name => orderDescending
                ? query.OrderByDescending(x => x.Name.Value)
                : query.OrderBy(x => x.Name.Value),
            ListTokensOrderKey.RoleType => orderDescending
                ? query.OrderByDescending(x => x.Role.Value)
                : query.OrderBy(x => x.Role.Value),
            ListTokensOrderKey.CreatedAt => orderDescending
                ? query.OrderByDescending(x => x.CreatedAt.Value)
                : query.OrderBy(x => x.CreatedAt.Value),
            ListTokensOrderKey.ExpiresAt => orderDescending
                ? query.OrderByDescending(x => x.ExpiresAt.Value)
                : query.OrderBy(x => x.ExpiresAt.Value),
            _ => throw new InvalidOperationException("The specified order key is not supported."),
        };

        var tokens = await query
            .Select(x => new TokenDto(
                x.Id,
                x.Name.Value,
                x.Role.Value,
                x.CreatedAt.Value,
                x.ExpiresAt.Value))
            .ToListAsync(cancellationToken);

        return tokens;
    }
}