using AdMeet.Models;
using Microsoft.EntityFrameworkCore;

namespace AdMeet.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // Define tus DbSets (tablas)
    public DbSet<User> Users { get; init; }
    public DbSet<Profile> Profile { get; init; }
    public DbSet<Category> Category { get; init; }
    public DbSet<Kpi> Kpi { get; init; }
}