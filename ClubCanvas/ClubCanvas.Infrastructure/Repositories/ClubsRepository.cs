using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ClubCanvas.Infrastructure.Data;
using ClubCanvas.Core;
using ClubCanvas.Core.Models;

namespace ClubCanvas.Infrastructure.Repositories;

public class ClubsRepository : IClubsRepository
{
    private readonly AppDbContext _context;

    public ClubsRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Club> GetAllClubs()
    {
        return _context.Clubs.ToList();
    }

    public Club GetClubById(int id)
    {
        return _context.Clubs.Find(id);
    }

    public void AddClub(Club club)
    {
        _context.Clubs.Add(club);
        _context.SaveChanges();
    }

    public void UpdateClub(Club club)
    {
        _context.Clubs.Update(club);
        _context.SaveChanges();
    }

    public void DeleteClub(int id)
    {
        var daniel = new ApplicationUser 
        { 
            Email = "daniel@example.com", 
            UserName = "daniel"
        };
        
        var sarah = new ApplicationUser 
        { 
            Email = "sarah@example.com", 
            UserName = "sarah"
        };

        return new List<Club>
        {
            new Club 
            { 
                Id = 1, 
                Name = "Photography Club", 
                Events = new List<Event> 
                { 
                    new Event 
                    { 
                        Id = 1, 
                        Name = "Photo Walk", 
                        Description = "Join us for a photography walk in the park", 
                        EventDate = DateTime.Now.AddDays(7), 
                        Location = "Central Park",
                        Attendees = new List<ApplicationUser>()
                    },
                    new Event 
                    { 
                        Id = 2, 
                        Name = "Portrait Workshop", 
                        Description = "Learn portrait photography techniques", 
                        EventDate = DateTime.Now.AddDays(14), 
                        Location = "Studio A",
                        Attendees = new List<ApplicationUser>()
                    }
                }, 
                Members = new List<ApplicationUser> { sarah, daniel }, 
                Owner = daniel, 
                Description = "We take photos.", 
                Image = "photography.png" 
            },
            new Club 
            { 
                Id = 2, 
                Name = "Gaming Club", 
                Events = new List<Event> 
                { 
                    new Event 
                    { 
                        Id = 3, 
                        Name = "Smash Night", 
                        Description = "Super Smash Bros tournament", 
                        EventDate = DateTime.Now.AddDays(3), 
                        Location = "Game Room",
                        Attendees = new List<ApplicationUser>()
                    }
                }, 
                Members = new List<ApplicationUser> { sarah }, 
                Owner = sarah, 
                Description = "Games and tournaments.", 
                Image = "gaming.png" 
            }
        };
    }
}

