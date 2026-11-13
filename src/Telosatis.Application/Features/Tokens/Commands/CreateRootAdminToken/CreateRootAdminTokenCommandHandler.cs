using Microsoft.EntityFrameworkCore;
using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Tokens.Contracts;
using Telosatis.Application.Services;
using Telosatis.Application.Services.Abstractions;
using Telosatis.Common;
using Telosatis.Domain.Token;

namespace Telosatis.Application.Features.Tokens.Commands.CreateRootAdminToken;

internal sealed class CreateRootAdminTokenCommandHandler(
    IApplicationDbContext dbContext,
    TimeProvider timeProvider)
    : ICommandHandler<CreateRootAdminTokenCommand, Result<TokenCreatedDto>>
{
    private const string RootAdminName = "Root";
    
    public async Task<Result<TokenCreatedDto>> Handle(CreateRootAdminTokenCommand request, CancellationToken cancellationToken)
    {
        if (await dbContext.Tokens.AnyAsync(x => x.Role.Value == TokenRoleType.RootAdmin, cancellationToken))
            return Errors.TokenWithRootAdminRoleAlreadyExists;

        var tokenResult = Token.Create(
            RootAdminName,
            TokenRoleType.RootAdmin,
            null,
            timeProvider);

        if (tokenResult.IsFailure)
            return tokenResult.Error;

        var (token, secret) = tokenResult.Value;

        dbContext.Tokens.Add(token);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new TokenCreatedDto(
            new TokenDto(
                token.Id,
                token.Name.Value,
                token.Role.Value,
                token.CreatedAt.Value,
                token.ExpiresAt.Value),
            secret);
    }
}