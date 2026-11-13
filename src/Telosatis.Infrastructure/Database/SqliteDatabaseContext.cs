using Microsoft.EntityFrameworkCore;
using Telosatis.Application.Services.Abstractions;
using Telosatis.Domain.Token;

namespace Telosatis.Infrastructure.Database;

public sealed class SqliteDatabaseContext : DbContext, IApplicationDbContext
{
    public DbSet<Token> Tokens => Set<Token>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqliteDatabaseContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
    
    public SqliteDatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected SqliteDatabaseContext()
    {
    }
}