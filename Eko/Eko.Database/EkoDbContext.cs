using Eko.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eko.Database;

public class EkoDbContext : DbContext, IEkoDbContext
{
    public DbSet<Person> Person { get; set; }
    
    public DbSet<Notification> Notification { get; set; }

    public EkoDbContext(DbContextOptions<EkoDbContext> options) : base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}

public interface IEkoDbContext
{
    DbSet<Person> Person { get; set; }
    
    DbSet<Notification> Notification { get; set; }
    Task<int> SaveChangesAsync();
}