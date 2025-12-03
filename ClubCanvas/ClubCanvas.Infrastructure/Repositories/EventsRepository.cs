using Microsoft.EntityFrameworkCore;
using ClubCanvas.Core.Interfaces;
using ClubCanvas.Core.Models;
using ClubCanvas.Infrastructure.Data;
namespace Assignment3.Infrastructure.Repositories;

public class EventsRepository : IEventsRepository
{
    private readonly AppDbContext _context;

    public EventsRepository(AppDbContext context)
    {
        _context = context;
    }
}