using Serilog;
using Telosatis.Api.Configuration;
using Telosatis.Application;
using Telosatis.Application.Options;
using Telosatis.Infrastructure.Authentication;
using Telosatis.Infrastructure.Database;
using Telosatis.Infrastructure.Extensions;
using Telosatis.Infrastructure.LinuxFileSystem;

namespace Telosatis.Api;

public static class Program
{
    public static int Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.ConfigureLogging();
        
        try
        {
            builder.ConfigureLargeFileTransfers();
        
            var serverOptions = builder.AddAndResolveOptions<ServerOptions>();
        
            builder.Services.AddLinuxFileSystemStorageBackend(builder.Environment);
        
            builder.Services.AddSqliteDatabase(builder.Environment);
        
            builder.Services.AddTelosatisApplication();

            builder.Services.AddJsonWebTokenAuth(builder.Configuration, serverOptions.Url);
        
            builder.Services.AddOpenApi(builder.Environment);

            builder.Services.AddPolicyBasedRateLimiting();

            builder.Services.AddReverseProxySupport(serverOptions.TrustedProxies, serverOptions.TrustedNetworks);

            builder.Services.AddGlobalErrorHandling();
        
            builder.Services.AddControllers(options =>
            {
                options.ModelValidatorProviders.Clear();
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });
        
            var app = builder.Build();

            app.UseLinuxFileSystemStorageBackend();
            
            app.UseSqliteDatabase();

            app.UseReverseProxySupport();

            app.UseLogging();
            
            app.UseGlobalErrorHandling();

            app.UseTelosatisWebApplication();
            
            app.UseRouting();
        
            app.UseJsonWebTokenAuth();
        
            app.UseRateLimiter();
        
            app.UseOpenApi();
        
            app.MapControllers();

            app.MapTelosatisWebApplication();

            app.Run();
            
            return 0;
        }
        catch (Exception ex) when (ex is not HostAbortedException)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
            
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}