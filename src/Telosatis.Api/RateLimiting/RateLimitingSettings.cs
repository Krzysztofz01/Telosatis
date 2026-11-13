namespace Telosatis.Api.RateLimiting;

internal static class RateLimitingSettings
{
    public const int AuthPermitLimit = 10;
    public static readonly TimeSpan AuthWindow = TimeSpan.FromMinutes(15);
    public const int AuthSegments = 5;
 
    public const int OnboardingPermitLimit = 20;
    public static readonly TimeSpan OnboardingWindow = TimeSpan.FromMinutes(1);
    public const int OnboardingSegments = 4;
}