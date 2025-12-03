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

    public List<Event> GetAllEvents()
    {
        return _context.Events.ToList();
    }

    public Event GetEventById(int id)
    {
        return _context.Events.Find(id);
    }
    
    public void AddEvent(Event e)
    {
        _context.Events.Add(e);
        _context.SaveChanges();
    }

    public void UpdateEvent(Event e)
    {
        _context.Events.Update(e);
        _context.SaveChanges();
    }

    public void DeleteEvent(int id)
    {
        var eventToDelete = _context.Events.Find(id);
        if (eventToDelete != null)
        {
            _context.Events.Remove(eventToDelete);
            _context.SaveChanges();
        }
    }
}