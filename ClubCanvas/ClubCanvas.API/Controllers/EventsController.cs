using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClubCanvas.Core;
using ClubCanvas.Core.Models;

namespace ClubCanvas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IClubsRepository _clubsRepository;

    public EventsController(IEventsRepository eventsRepository, IClubsRepository clubsRepository)
    {
        _eventsRepository = eventsRepository;
        _clubsRepository = clubsRepository;
    }

    // GET: api/events?clubId=1
    // Gets all events, optionally filtered by club
    [HttpGet]
    public async Task<ActionResult<List<Event>>> GetEvents([FromQuery] int? clubId = null)
    {
        if (clubId.HasValue)
        {
            // Get events for a specific club
            var club = await _clubsRepository.GetClubByIdAsync(clubId.Value);
            if (club == null)
            {
                return NotFound($"Club with ID {clubId.Value} not found");
            }
            return Ok(club.Events ?? new List<Event>());
        }

        // Get all events
        var allEvents = await _eventsRepository.GetAllEventsAsync();
        return Ok(allEvents);
    }

    // GET: api/events/5
    // Gets a specific event
    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEventById(int id)
    {
        var eventItem = await _eventsRepository.GetEventByIdAsync(id);
        if (eventItem == null)
        {
            return NotFound($"Event with ID {id} not found");
        }

        return Ok(eventItem);
    }

    // POST: api/events
    [HttpPost]
    [Authorize] // Requires JWT token
    public async Task<ActionResult<Event>> CreateEvent([FromBody] Event eventItem)
    {
        if (eventItem == null)
        {
            return BadRequest("Event data is required");
        }

        await _eventsRepository.AddEventAsync(eventItem);
        return CreatedAtAction(nameof(GetEventById), new { id = eventItem.Id }, eventItem);
    }

    // PUT: api/events/5
    [HttpPut("{id}")]
    [Authorize] // Requires JWT token
    public async Task<ActionResult> UpdateEvent(int id, [FromBody] Event eventItem)
    {
        if (eventItem == null)
        {
            return BadRequest("Event data is required");
        }

        if (id != eventItem.Id)
        {
            return BadRequest("Event ID mismatch");
        }

        var existingEvent = await _eventsRepository.GetEventByIdAsync(id);
        if (existingEvent == null)
        {
            return NotFound($"Event with ID {id} not found");
        }

        await _eventsRepository.UpdateEventAsync(eventItem);
        return NoContent();
    }

    // DELETE: api/events/5
    [HttpDelete("{id}")]
    [Authorize] // Requires JWT token
    public async Task<ActionResult> DeleteEvent(int id)
    {
        var eventItem = await _eventsRepository.GetEventByIdAsync(id);
        if (eventItem == null)
        {
            return NotFound($"Event with ID {id} not found");
        }

        await _eventsRepository.DeleteEventAsync(id);
        return NoContent();
    }
}

