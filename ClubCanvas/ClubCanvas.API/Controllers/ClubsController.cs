using Microsoft.AspNetCore.Mvc;
using ClubCanvas.Core;
using ClubCanvas.Core.Models;

namespace ClubCanvas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubsController : ControllerBase
{
    private readonly IClubsRepository _clubsRepository;

    // Constructor injection - ASP.NET Core gives us the repository automatically
    public ClubsController(IClubsRepository clubsRepository)
    {
        _clubsRepository = clubsRepository;
    }

    // GET: api/clubs
    [HttpGet]
    public async Task<ActionResult<List<Club>>> GetAllClubs()
    {
        var clubs = await _clubsRepository.GetAllClubsAsync();
        return Ok(clubs);
    }

    // GET: api/clubs/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Club>> GetClubById(int id)
    {
        var club = await _clubsRepository.GetClubByIdAsync(id);
        
        if (club == null)
        {
            return NotFound($"Club with ID {id} not found");
        }
        
        return Ok(club);
    }

    // POST: api/clubs
    // Creates a new club
    [HttpPost]
    public async Task<ActionResult<Club>> CreateClub([FromBody] Club club)
    {
        if (club == null)
        {
            return BadRequest("Club data is required");
        }

        await _clubsRepository.AddClubAsync(club);
        
        // Return 201 Created with the new club
        return CreatedAtAction(nameof(GetClubById), new { id = club.Id }, club);
    }

    // PUT: api/clubs/5
    // Updates an existing club
    [HttpPut("{id}")]
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
}

