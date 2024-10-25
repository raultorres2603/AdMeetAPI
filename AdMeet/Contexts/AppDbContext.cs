using AdMeet.Models;
using Microsoft.EntityFrameworkCore;

namespace AdMeet.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Define tus DbSets (tablas)
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configuración adicional, si es necesario
    }
}