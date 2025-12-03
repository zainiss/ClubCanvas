using Microsoft.EntityFrameworkCore;
using ClubCanvas.Core.Models;
using ClubCanvas.Infrastructure.Repositories;

namespace ClubCanvas.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<Event> Events { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }
}