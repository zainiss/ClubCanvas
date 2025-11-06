using System.Collections.Generic;
using System.Linq;
using ClubCanvas.mvc.Models;

namespace ClubCanvas.mvc.Database;

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
        return new List<Club>
        {
            new Club { Id = 1, Name = "Photography Club", Events = new List<string> { "Photo Walk", "Portrait Workshop" }, Members = new List<string> { "sarah", "daniel" }, Owner = "daniel", Description = "We take photos.", Image = "photography.png" },
            new Club { Id = 2, Name = "Gaming Club", Events = new List<string> { "Smash Night" }, Members = new List<string> { "sarah" }, Owner = "sarah", Description = "Games and tournaments.", Image = "gaming.png" }
        };
    }
}