using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telosatis.Application.Services.Abstractions;

namespace Telosatis.Infrastructure.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddSqliteDatabase(
        this IServiceCollection services,
        IWebHostEnvironment env)
    {
        services.AddDbContext<SqliteDatabaseContext>(options =>
        {
            var databasePath = env.IsDevelopment()
                ? Constants.SqliteDatabaseDevelopmentPath
                : Constants.SqliteDatabaseProductionPath;

            var connectionString = Constants.SqliteConnectionString(databasePath);
            
            options.UseSqlite(connectionString, sqlite =>
            {
                sqlite.CommandTimeout(Constants.Timeout);
            });

            options.EnableDetailedErrors(env.IsDevelopment());

            options.EnableSensitiveDataLogging(false);

            if (env.IsDevelopment())
                options.EnableDetailedErrors();
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<SqliteDatabaseContext>());
        
        return services;
    }

    public static IApplicationBuilder UseSqliteDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SqliteDatabaseContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SqliteDatabaseContext>>();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        
        var pendingMigrations = dbContext.Database
            .GetPendingMigrations()
            .Count();
        
        if (pendingMigrations > 0)
        {
            logger.LogInformation("Executing {MigrationCount} database migrations", pendingMigrations);
            dbContext.Database.Migrate();
            logger.LogInformation("Database migrations has been applied");
        }
        
        var databasePath = env.IsDevelopment()
            ? Constants.SqliteDatabaseDevelopmentPath
            : Constants.SqliteDatabaseProductionPath;
        
        logger.LogInformation("Using database located at: {DatabasePath}", databasePath);
        return app;
    }
}