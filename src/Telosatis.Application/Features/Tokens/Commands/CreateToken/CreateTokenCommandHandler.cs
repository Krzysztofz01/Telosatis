using Microsoft.EntityFrameworkCore;
using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Tokens.Contracts;
using Telosatis.Application.Services;
using Telosatis.Application.Services.Abstractions;
using Telosatis.Common;
using Telosatis.Domain.Token;

namespace Telosatis.Application.Features.Tokens.Commands.CreateToken;

internal sealed class CreateTokenCommandHandler(
    IApplicationDbContext dbContext,
    TimeProvider timeProvider)
    : ICommandHandler<CreateTokenCommand, Result<TokenCreatedDto>>
{
    public async Task<Result<TokenCreatedDto>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        if (await dbContext.Tokens.AnyAsync(x => x.Name.Value == request.Name, cancellationToken))
            return Errors.TokenWithGivenNameAlreadyExists;

        if (request.RoleType == TokenRoleType.RootAdmin)
            return Errors.TokenWithRootAdminRoleCanNotBeCreated;

        var tokenResult = Token.Create(
            request.Name,
            request.RoleType,
            request.ExpiresAt,
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