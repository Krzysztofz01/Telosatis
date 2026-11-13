using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Telosatis.Domain.Token;

namespace Telosatis.Application.Services.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Token> Tokens { get; }

    DatabaseFacade Database { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}