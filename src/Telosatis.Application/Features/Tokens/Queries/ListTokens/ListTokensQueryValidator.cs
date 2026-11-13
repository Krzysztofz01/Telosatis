using FluentValidation;
using Telosatis.Application.Extensions;

namespace Telosatis.Application.Features.Tokens.Queries.ListTokens;

public sealed class ListTokensQueryValidator : AbstractValidator<ListTokensQuery>
{
    public ListTokensQueryValidator()
    {
        RuleFor(x => x.OrderKey)
            .IsInEnum()
            .WithError(Errors.ListTokensOrderKeyNotSupported);

        RuleFor(x => x.OrderDescending);
    }
}