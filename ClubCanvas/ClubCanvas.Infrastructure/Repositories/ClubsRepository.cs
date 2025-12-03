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
        var clubToDelete = _context.Clubs.Find(id);
        if (clubToDelete != null)
        {
            _context.Clubs.Remove(clubToDelete);
            _context.SaveChanges();
        }
    }
}
