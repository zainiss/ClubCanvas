using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClubCanvas.Core;
using ClubCanvas.Core.Models;
using ClubCanvas.Infrastructure.Data;

namespace ClubCanvas.Infrastructure.Repositories;

public class EventsRepository : IEventsRepository
{
    private readonly AppDbContext _context;

    public EventsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetAllEventsAsync()
    {
        return await _context.Events
            .Include(e => e.Attendees)
            .ToListAsync();
    }

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        return await _context.Events
            .Include(e => e.Attendees)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
    
    public async Task AddEventAsync(Event e)
    {
        _context.Events.Add(e);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEventAsync(Event e)
    {
        _context.Events.Update(e);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEventAsync(int id)
    {
        var eventToDelete = await _context.Events.FindAsync(id);
        if (eventToDelete != null)
        {
            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();
        }
    }
}