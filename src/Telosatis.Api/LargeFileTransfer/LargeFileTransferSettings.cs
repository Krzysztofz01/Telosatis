namespace Telosatis.Api.LargeFileTransfer;

internal static class LargeFileTransferSettings
{
    public const long DefaultMaxRequestBodyBytes = 32L * 1024 * 1024; // 32MB
    public const long MaxUploadBytes = 100L * 1024 * 1024 * 1024; // 100GB
    public const long MinDataRateBytesPerSecond = 1024; // 1KB
    public const long MinDataRateGraceSeconds = 30;
    public const long CopyBufferBytes = 1024 * 1024; // 1MB

    public const int KeepAliveTimeoutSeconds = 2 * 60;
    public const int RequestHeaderTimeoutTimeoutSeconds = 30;

    public const int InitialConnectionWindowSize = 4 * 1024 * 1024; // 4MB
    public const int InitialStreamWindowSize = 2 * 1024 * 1024; // 2MB

    public const int FormMemoryBufferThreshold = 64 * 1024; // 64KB
}