using Microsoft.EntityFrameworkCore;
using Telosatis.Application.Abstractions;
using Telosatis.Application.Services;
using Telosatis.Application.Services.Abstractions;
using Telosatis.Common;
using Telosatis.Domain.Token;

namespace Telosatis.Application.Features.Tokens.Commands.DeleteToken;

internal sealed class DeleteTokenCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteTokenCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await dbContext.Tokens
            .SingleOrDefaultAsync(x => x.Id == request.TokenId, cancellationToken);

        if (token is null)
            return Errors.TokenNotFound;

        if (token.Role.Value == TokenRoleType.RootAdmin)
            return Errors.TokenWithRootAdminRoleCanNotBeDeleted;

        dbContext.Tokens.Remove(token);
        await dbContext.SaveChangesAsync(cancellationToken);

        return request.TokenId;
    }
}