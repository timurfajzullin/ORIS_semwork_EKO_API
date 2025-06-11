using Eko.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;

namespace Eko.Database;

public class EkoDbContext : DbContext, IEkoDbContext
{
    public DbSet<Person?> Person { get; set; }
    
    public DbSet<Notification> Notification { get; set; }
    
    public DbSet<Profile> Profile { get; set; }
    
    public DbSet<Chat> Chat { get; set; }

    public EkoDbContext(DbContextOptions<EkoDbContext> options) : base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasData(new Person
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@gmail.com",
                Password = "admin",
                IsAdmin = true
            });
        
        modelBuilder.Entity<Chat>().Property(x => x.Id).HasColumnType("uuid");
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}

public interface IEkoDbContext
{
    DbSet<Person?> Person { get; set; }
    
    DbSet<Notification> Notification { get; set; }
    
    DbSet<Profile> Profile { get; set; }
    
    DbSet<Chat> Chat { get; set; }
    Task<int> SaveChangesAsync();
}