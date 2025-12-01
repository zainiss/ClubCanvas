using System;
using System.Collections.Generic;
using System.Linq;
using ClubCanvas.Core;
using ClubCanvas.Core.Models;

namespace ClubCanvas.Infrastructure;

public class ClubsRepository : IClubsRepository
{
    private readonly List<Club> _clubs;

    public ClubsRepository()
    {
        _clubs = ClubsSeed.Create();
    }

    public List<Club> GetAllClubs()
    {
        return _clubs;
    }

    public Club GetClubById(int id)
    {
        return _clubs.FirstOrDefault(c => c.Id == id);
    }

    public void DeleteClub(int id)
    {
        var existing = _clubs.FirstOrDefault(c => c.Id == id);
        if (existing != null)
        {
            _clubs.Remove(existing);
        }
    }

    public void AddClub(Club club)
    {
        if (club == null)
        {
            return;
        }

        if (club.Id == 0)
        {
            var nextId = _clubs.Count == 0 ? 1 : _clubs.Max(c => c.Id) + 1;
            club.Id = nextId;
        }

        _clubs.Add(club);
    }

    public void UpdateClub(Club club)
    {
        if (club == null)
        {
            return;
        }

        var existing = _clubs.FirstOrDefault(c => c.Id == club.Id);
        if (existing == null)
        {
            return;
        }

        existing.Name = club.Name;
        existing.Events = club.Events;
        existing.Members = club.Members;
        existing.Owner = club.Owner;
        existing.Description = club.Description;
        existing.Image = club.Image;
    }
}

internal static class ClubsSeed
{
    public static List<Club> Create()
    {
        var daniel = new User 
        { 
            Email = "daniel@example.com", 
            Username = "daniel", 
            Password = "password123" 
        };
        
        var sarah = new User 
        { 
            Email = "sarah@example.com", 
            Username = "sarah", 
            Password = "password123" 
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
                        Attendees = new List<User>()
                    },
                    new Event 
                    { 
                        Id = 2, 
                        Name = "Portrait Workshop", 
                        Description = "Learn portrait photography techniques", 
                        EventDate = DateTime.Now.AddDays(14), 
                        Location = "Studio A",
                        Attendees = new List<User>()
                    }
                }, 
                Members = new List<string> { "sarah", "daniel" }, 
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
                        Attendees = new List<User>()
                    }
                }, 
                Members = new List<string> { "sarah" }, 
                Owner = sarah, 
                Description = "Games and tournaments.", 
                Image = "gaming.png" 
            }
        };
    }
}