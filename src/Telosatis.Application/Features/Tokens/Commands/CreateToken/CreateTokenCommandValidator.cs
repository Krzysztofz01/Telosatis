using FluentValidation;
using Telosatis.Application.Extensions;

namespace Telosatis.Application.Features.Tokens.Commands.CreateToken;

public sealed class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
{
    public CreateTokenCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(Errors.TokenNameCanNotBeEmpty);

        RuleFor(x => x.RoleType)
            .IsInEnum()
            .WithError(Errors.TokenRoleTypeIsNotSupported);

        RuleFor(x => x.ExpiresAt)
            .NotEmpty()
            .WithError(Errors.TokenExpiresAtCanNotBeEmpty);
    }
}