using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClubCanvas.Core;
using ClubCanvas.Core.Models;
using ClubCanvas.Shared.DTOs;
using System.Net.Mail;
using System.Net;

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
    public async Task<ActionResult<List<CreateEventDto>>> GetEvents([FromQuery] int? clubId = null)
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
        return Ok(allEvents.Select(c => new CreateEventDto {
            Id = c.Id,
            Name = c.Name ?? string.Empty,
            Description = c.Description,
            Location = c.Location,
            EventDate = c.EventDate ?? DateTime.MinValue,
            ClubId = c.ClubId
        }).ToList());
    }

    // GET: api/events/5
    // Gets a specific event
    [HttpGet("{id}")]
    public async Task<ActionResult<CreateEventDto>> GetEventById(int id)
    {
        var eventItem = await _eventsRepository.GetEventByIdAsync(id);
        if (eventItem == null)
        {
            return NotFound($"Event with ID {id} not found");
        }

        // Map attendees to DTOs
        var attendeesDto = eventItem.Attendees?.Select(a => new UserDto
        {
            Username = a.UserName ?? string.Empty,
            Email = a.Email ?? string.Empty
        }).ToList() ?? new List<UserDto>();

        return Ok(new CreateEventDto
        {
            Id = eventItem.Id,
            Name = eventItem.Name ?? string.Empty,
            Description = eventItem.Description,
            EventDate = eventItem.EventDate ?? DateTime.MinValue,
            Location = eventItem.Location,
            ClubId = eventItem.ClubId,
            Attendees = attendeesDto
        });
    }

    // POST: api/events
    [HttpPost]
    [Authorize] // Requires JWT token
    public async Task<ActionResult<Event>> CreateEvent([FromBody] CreateEventDto eventItem)
    {
        if (eventItem == null)
        {
            return BadRequest("Event data is required");
        }

        var e = new Event
        {
            Name = eventItem.Name,
            Description = eventItem.Description,
            Location = eventItem.Location,
            EventDate = eventItem.EventDate,
            ClubId = eventItem.ClubId
        };

        await _eventsRepository.AddEventAsync(e);
        return CreatedAtAction(nameof(GetEventById), new { id = e.Id }, e);
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

    // POST: api/events/{id}/register
    [HttpPost("{id}/register")]
    [Authorize] // Requires JWT token
    public async Task<ActionResult> RegisterForEvent(int id, [FromServices] UserManager<ApplicationUser> userManager, [FromServices] Infrastructure.Data.AppDbContext context)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized("User not authenticated");
        }

        var eventItem = await context.Events
            .Include(e => e.Attendees)
            .FirstOrDefaultAsync(e => e.Id == id);
            
        if (eventItem == null)
        {
            return NotFound($"Event with ID {id} not found");
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        // Check if user is already registered
        if (eventItem.Attendees != null && eventItem.Attendees.Any(a => a.Id == userId))
        {
            return BadRequest("User is already registered for this event");
        }

        // Add user to attendees - EF Core will handle the many-to-many relationship
        if (eventItem.Attendees == null)
        {
            eventItem.Attendees = new List<ApplicationUser>();
        }
        eventItem.Attendees.Add(user);

        await context.SaveChangesAsync();

        await SendEmailAsync(user.Email, "Registered for an Event!", $"You have registered for {eventItem.Name} at {eventItem.Location} on {eventItem.EventDate}");
        
        return Ok(new { message = "Successfully registered for event" });
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        //passforCCemail1
        using var message = new MailMessage();
        message.From = new MailAddress("SheridanCSAssignments@gmail.com", "ClubCanvas");
        message.To.Add(toEmail);
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        using var client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("SheridanCSAssignments@gmail.com", "wvpq ahen imwc jurl"),
            EnableSsl = true
        };

        message.To.Add(toEmail);

        await client.SendMailAsync(message);
    }
}