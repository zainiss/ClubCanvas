using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public async Task<List<Club>> GetAllClubsAsync()
    {
        return await _context.Clubs
            .Include(c => c.Events)
            .Include(c => c.Members)
            .Include(c => c.Owner)
            .ToListAsync();
    }

    public async Task<Club?> GetClubByIdAsync(int id)
    {
        return await _context.Clubs
            .Include(c => c.Events)
            .Include(c => c.Members)
            .Include(c => c.Owner)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddClubAsync(Club club)
    {
        _context.Clubs.Add(club);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateClubAsync(Club club)
    {
        _context.Clubs.Update(club);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteClubAsync(int id)
    {
        var clubToDelete = await _context.Clubs.FindAsync(id);
        if (clubToDelete != null)
        {
            _context.Clubs.Remove(clubToDelete);
            await _context.SaveChangesAsync();
        }
    }
}

