using Microsoft.AspNetCore.Mvc;
using ClubCanvas.Core;
using ClubCanvas.Core.Models;

namespace ClubCanvas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IClubsRepository _clubsRepository;

    public EventsController(IClubsRepository clubsRepository)
    {
        _clubsRepository = clubsRepository;
    }

    // GET: api/events?clubId=1
    // Gets all events, optionally filtered by club
    [HttpGet]
    public ActionResult<List<Event>> GetEvents([FromQuery] int? clubId = null)
    {
        if (clubId.HasValue)
        {
            // Get events for a specific club
            var club = _clubsRepository.GetClubById(clubId.Value);
            if (club == null)
            {
                return NotFound($"Club with ID {clubId.Value} not found");
            }
            return Ok(club.Events ?? new List<Event>());
        }

        // Get all events from all clubs
        var allClubs = _clubsRepository.GetAllClubs();
        var allEvents = allClubs
            .Where(c => c.Events != null)
            .SelectMany(c => c.Events)
            .ToList();
        
        return Ok(allEvents);
    }

    // GET: api/events/5?clubId=1
    // Gets a specific event from a club
    [HttpGet("{eventId}")]
    public ActionResult<Event> GetEventById(int eventId, [FromQuery] int clubId)
    {
        var club = _clubsRepository.GetClubById(clubId);
        if (club == null)
        {
            return NotFound($"Club with ID {clubId} not found");
        }

        var eventItem = club.Events?.FirstOrDefault(e => e.Id == eventId);
        if (eventItem == null)
        {
            return NotFound($"Event with ID {eventId} not found in club {clubId}");
        }

        return Ok(eventItem);
    }
}

