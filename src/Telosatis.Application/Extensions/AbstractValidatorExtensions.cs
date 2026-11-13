using FluentValidation;

namespace Telosatis.Application.Extensions;

public static class AbstractValidatorExtensions
{
    public static IRuleBuilderOptions<T, T> RuleForSelf<T>(this AbstractValidator<T> @this, Func<T, bool> predicate)
    {
        return @this.RuleFor(x => x).Must(predicate);
    }
}