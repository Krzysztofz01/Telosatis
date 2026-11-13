using FluentValidation;
using Telosatis.Application.Extensions;

namespace Telosatis.Application.Features.Authentication.Commands.Reauthenticate;

public sealed class ReauthenticateCommandValidator : AbstractValidator<ReauthenticateCommand>
{
    public ReauthenticateCommandValidator()
    {
        RuleFor(x => x.TokenId)
            .IsGuidId()
            .WithError(Errors.TokenIdCanNotBeEmpty);
    }
}