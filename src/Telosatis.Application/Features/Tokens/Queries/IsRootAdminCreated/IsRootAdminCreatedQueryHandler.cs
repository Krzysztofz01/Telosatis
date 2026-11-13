using Microsoft.EntityFrameworkCore;
using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Tokens.Contracts;
using Telosatis.Application.Services;
using Telosatis.Application.Services.Abstractions;
using Telosatis.Common;
using Telosatis.Domain.Token;

namespace Telosatis.Application.Features.Tokens.Queries.IsRootAdminCreated;

internal sealed class IsRootAdminCreatedQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<IsRootAdminCreatedQuery, Result<TokenRootAdminCreatedDto>>
{
    public async Task<Result<TokenRootAdminCreatedDto>> Handle(IsRootAdminCreatedQuery request, CancellationToken cancellationToken)
    {
        var isCreated = await dbContext.Tokens
            .AnyAsync(x => x.Role.Value == TokenRoleType.RootAdmin, cancellationToken);

        return new TokenRootAdminCreatedDto(isCreated);
    }
}