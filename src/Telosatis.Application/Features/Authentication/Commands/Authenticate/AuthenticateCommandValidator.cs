using FluentValidation;
using Telosatis.Application.Extensions;

namespace Telosatis.Application.Features.Authentication.Commands.Authenticate;

public sealed class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
{
    public AuthenticateCommandValidator()
    {
        RuleFor(x => x.Secret)
            .NotEmpty()
            .WithError(Errors.SecretCanNotBeEmpty);
    }
}