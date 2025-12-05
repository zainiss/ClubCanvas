using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ClubCanvas.Shared.DTOs;
using ClubCanvas.Core;
using ClubCanvas.Core.Models;

namespace ClubCanvas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubsController : ControllerBase
{
    private readonly IClubsRepository _clubsRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    // Constructor injection - ASP.NET Core gives us the repository automatically
    public ClubsController(IClubsRepository clubsRepository, UserManager<ApplicationUser> userManager)
    {
        _clubsRepository = clubsRepository;
        _userManager = userManager;
    }

    // GET: api/clubs
    [HttpGet]
    public async Task<ActionResult<List<CreateClubDto>>> GetAllClubs()
    {
        var clubs = await _clubsRepository.GetAllClubsAsync();
        return Ok(clubs.Select(c => new CreateClubDto {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Image = c.Image,
            OwnerId = c.OwnerId
        }).ToList());
    }

    // GET: api/clubs/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CreateClubDto>> GetClubById(int id)
    {
        var club = await _clubsRepository.GetClubByIdAsync(id);
        
        if (club == null)
        {
            return NotFound($"Club with ID {id} not found");
        }

        var owner = await _userManager.FindByIdAsync(club.OwnerId);
        
        return Ok(new CreateClubDto
        {
            Id = club.Id,
            Name = club.Name,
            Description = club.Description,
            Image = club.Image,
            OwnerId = club.OwnerId,
            OwnerName = owner.UserName,
            OwnerEmail = owner.Email
        });
    }

    // POST: api/clubs
    // Creates a new club
    [HttpPost]
    [Authorize] // Requires JWT token
    public async Task<ActionResult<Club>> CreateClub([FromBody] CreateClubDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Get the current user from JWT token
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized("User not authenticated");
        }

        var currentUser = await _userManager.FindByIdAsync(userId);
        if (currentUser == null)
        {
            return Unauthorized("User not found");
        }

        // Map DTO to Club entity
        var club = new Club
        {
            Name = dto.Name,
            Description = dto.Description,
            Image = dto.Image ?? string.Empty,
            Owner = currentUser,
            Members = new List<ApplicationUser>(),
            Events = dto.Events?.Select(e => new Event
            {
                Name = e.Name,
                Description = e.Description,
                EventDate = e.EventDate,
                Location = e.Location,
                Attendees = new List<ApplicationUser>()
            }).ToList() ?? new List<Event>()
        };

        await _clubsRepository.AddClubAsync(club);
        
        // Return 201 Created with the new club
        return CreatedAtAction(nameof(GetClubById), new { id = club.Id }, club);
    }

    // PUT: api/clubs/5
    // Updates an existing club
    [HttpPut("{id}")]
    [Authorize] // Requires JWT token
    public async Task<ActionResult> UpdateClub(int id, [FromBody] Club club)
    {
        if (club == null)
        {
            return BadRequest("Club data is required");
        }

        if (id != club.Id)
        {
            return BadRequest("Club ID mismatch");
        }

        var existingClub = await _clubsRepository.GetClubByIdAsync(id);
        if (existingClub == null)
        {
            return NotFound($"Club with ID {id} not found");
        }

        await _clubsRepository.UpdateClubAsync(club);
        
        return NoContent(); // 204 No Content - standard for successful PUT
    }

    // DELETE: api/clubs/5
    // Deletes a club
    [HttpDelete("{id}")]
    [Authorize] // Requires JWT token
    public async Task<ActionResult> DeleteClub(int id)
    {
        var club = await _clubsRepository.GetClubByIdAsync(id);
        if (club == null)
        {
            return NotFound($"Club with ID {id} not found");
        }

        await _clubsRepository.DeleteClubAsync(id);
        
        return NoContent(); // 204 No Content - standard for successful DELETE
    }

    [HttpPost]
    [Authorize]
    [Route("JoinClub/{clubId}/{userId}")]
     public async Task<ActionResult> JoinClub(int clubId, string userId)
    {

        var existingClub = await _clubsRepository.GetClubByIdAsync(clubId);
        if (existingClub == null)
        {
            return NotFound($"Club with ID {clubId} not found");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"User with ID {userId} not found");
        }

        existingClub.AddMember(user);

        await _clubsRepository.UpdateClubAsync(existingClub);
        
        return NoContent(); // 204 No Content - standard for successful PUT
    }
}

