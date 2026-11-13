using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Telosatis.Infrastructure.Extensions;

internal static class OptionsBuilderExtensions
{
    public static OptionsBuilder<TOptions> ValidateOnStart<TOptions>(this OptionsBuilder<TOptions> @this, bool enabled)
        where TOptions : class
    {
        return enabled ? @this.ValidateOnStart() : @this;
    }
}