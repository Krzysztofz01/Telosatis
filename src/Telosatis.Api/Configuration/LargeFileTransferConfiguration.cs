using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Telosatis.Api.LargeFileTransfer;

namespace Telosatis.Api.Configuration;

internal static class LargeFileTransferConfiguration
{
    public static WebApplicationBuilder ConfigureLargeFileTransfers(this WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(kestrel =>
        {
            kestrel.Limits.MaxRequestBodySize = LargeFileTransferSettings.DefaultMaxRequestBodyBytes;

            var rate = new MinDataRate(
                LargeFileTransferSettings.MinDataRateBytesPerSecond,
                TimeSpan.FromSeconds(LargeFileTransferSettings.MinDataRateGraceSeconds));
            
            kestrel.Limits.MinRequestBodyDataRate = rate;
            kestrel.Limits.MinResponseDataRate = rate;

            kestrel.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(LargeFileTransferSettings.KeepAliveTimeoutSeconds);
            kestrel.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(LargeFileTransferSettings.RequestHeaderTimeoutTimeoutSeconds);

            kestrel.Limits.Http2.InitialConnectionWindowSize = LargeFileTransferSettings.InitialConnectionWindowSize;
            kestrel.Limits.Http2.InitialStreamWindowSize = LargeFileTransferSettings.InitialStreamWindowSize;
        });

        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = LargeFileTransferSettings.MaxUploadBytes;
            options.MemoryBufferThreshold = LargeFileTransferSettings.FormMemoryBufferThreshold;
        });
        
        return builder;
    }
}