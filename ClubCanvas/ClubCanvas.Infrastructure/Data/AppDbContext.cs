using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClubCanvas.Core.Models;

namespace ClubCanvas.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public DbSet<Club> Clubs { get; set; }
    public DbSet<Event> Events { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configure Identity table names (optional, for consistency)
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");
        });
        
        builder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable("Roles");
        });
    }
}