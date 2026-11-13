using FluentValidation;
using Telosatis.Application.Extensions;

namespace Telosatis.Application.Features.Tokens.Commands.DeleteToken;

public sealed class DeleteTokenCommandValidator : AbstractValidator<DeleteTokenCommand>
{
    public DeleteTokenCommandValidator()
    {
        RuleFor(x => x.TokenId)
            .IsGuidId()
            .WithError(Errors.TokeIdCanNotBeEmpty);
    }    
}