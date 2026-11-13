using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telosatis.Domain.FileSystem;
using Telosatis.Infrastructure.Exceptions;

namespace Telosatis.Infrastructure.LinuxFileSystem;

public static class DependencyInjection
{
    public static IServiceCollection AddLinuxFileSystemStorageBackend(this IServiceCollection services, IWebHostEnvironment env)
    {
        if (!OperatingSystem.IsLinux())
            throw new InfrastructureException("Can not register this storage backend due to unsupported operating system.");

        // NOTE: Resolve for validation purposes
        _ = GetRootPath(env.IsDevelopment());
        
        services.AddSingleton<IStorageBackend, LinuxFileSystemStorageBackend>(provider =>
        {
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var environment = provider.GetRequiredService<IWebHostEnvironment>();
            
            var storageBackendResult = LinuxFileSystemStorageBackend.CreateStorageBackend(
                GetRootPath(environment.IsDevelopment()),
                loggerFactory);

            if (storageBackendResult.IsFailure)
                throw new InfrastructureException($"Failed to create the storage backend due to error: {storageBackendResult.Error.Id}");

            return storageBackendResult.Value;
        });
        
        return services;
    }
    
    public static IApplicationBuilder UseLinuxFileSystemStorageBackend(
        this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<IStorageBackend>>();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        
        // NOTE: Preload storage backend
        _ = scope.ServiceProvider.GetRequiredService<IStorageBackend>();

        var path = GetRootPath(env.IsDevelopment());
        logger.LogInformation("Using storage backend with root location at: {DatabasePath}", path);
        return app;
    }

    private static string GetRootPath(bool isDevelopment)
    {
        if (isDevelopment)
        {
            var path = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "telosatis", "data");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }
        
        if (!FileSystemRoot.EnsureRootPath())
            throw new InfrastructureException("The file system root path could not be initialized.");

        return FileSystemRoot.RootPath;
    }
}