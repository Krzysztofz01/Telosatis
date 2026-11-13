using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Telosatis.Infrastructure.Database;

public sealed class DesignTimeSqliteDatabaseContextFactory : IDesignTimeDbContextFactory<SqliteDatabaseContext>
{
    public SqliteDatabaseContext CreateDbContext(string[] args)
    {
        var connectionString = Constants.SqliteConnectionString(Constants.SqliteDatabaseDevelopmentPath);
        
        var optionsBuilder = new DbContextOptionsBuilder<SqliteDatabaseContext>();
        optionsBuilder.UseSqlite(connectionString);

        return new SqliteDatabaseContext(optionsBuilder.Options);
    }
}