using System.Net.Mail;
using FluentValidation;
using Telosatis.Application.Errors;

namespace Telosatis.Application.Extensions;

internal static class RuleBuilderOptionsExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> @this, ValidationError error)
    {
        return @this.WithErrorCode(error.Id).WithMessage(error.Description);
    }

    public static IRuleBuilderOptions<T, Guid> IsGuidId<T>(this IRuleBuilder<T, Guid> @this)
    {
        return @this.Must(x => x != Guid.Empty);
    }

    public static IRuleBuilderOptions<T, string> IsEmailAddress<T>(this IRuleBuilder<T, string> @this)
    {
        return @this.Must(x =>
        {
            var trimmedEmail = x.Trim();

            if (trimmedEmail.EndsWith('.'))
                return false;

            try
            {
                var parsedEmail = new MailAddress(x);
                return parsedEmail.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        });
    }

    public static IRuleBuilderOptions<T, DateTime> IsUtcDateTime<T>(this IRuleBuilder<T, DateTime> @this)
    {
        return @this.Must(x => x.Kind == DateTimeKind.Utc);
    }

    public static IRuleBuilderOptions<T, string?> IsAbsolutePathString<T>(this IRuleBuilder<T, string?> @this)
    {
        return @this.Must(x =>
        {
            if (string.IsNullOrWhiteSpace(x))
                return false;

            if (!Path.IsPathRooted(x))
                return false;

            return true;
        });
    }
}
