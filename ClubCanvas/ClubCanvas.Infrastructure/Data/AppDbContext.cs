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

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.HasMany(e => e.Events).WithOne(e => e.Club).HasForeignKey(e => e.ClubId);
            entity.HasMany(e => e.Members);
            entity.HasOne(e => e.Owner).WithMany(e => e.OwnedClubs).HasForeignKey(entity => entity.OwnerId);
            entity.Property(e => e.Image).IsRequired();
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.EventDate).IsRequired();
            entity.Property(e => e.Location).IsRequired().HasMaxLength(100);
            entity.HasMany(e => e.Attendees);
            entity.Property(e => e.ClubId).IsRequired();
            entity.HasOne(e => e.Club).WithMany(e => e.Events).HasForeignKey(e => e.ClubId);
        });
    }
}