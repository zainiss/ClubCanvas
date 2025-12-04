using Microsoft.AspNetCore.Mvc;
using ClubCanvas.Core.Models;
using System.Net.Http.Json;
using ClubCanvas.Core;
using ClubCanvas.web.Models;

namespace ClubCanvas.web.Controllers;

public class ClubsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IClubsRepository _clubs;

    public ClubsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [Route("Clubs")]
    public async Task<IActionResult> Clubs()
    {
        var httpClient = _httpClientFactory.CreateClient("ClubCanvasAPI");
        
        try
        {
            var clubs = await httpClient.GetFromJsonAsync<List<Club>>("clubs");
            return View(clubs ?? new List<Club>());
        }
        catch (HttpRequestException ex)
        {
            // Log the error for debugging
            Console.WriteLine($"Error calling API: {ex.Message}");
            // API is not available - return empty list
            return View(new List<Club>());
        }
        catch (Exception ex)
        {
            // Log any other errors
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return View(new List<Club>());
        }
    }

    public async Task<IActionResult> ClubDetails(int id)
    {
        var httpClient = _httpClientFactory.CreateClient("ClubCanvasAPI");
        
        try
        {
            var club = await httpClient.GetFromJsonAsync<Club>($"clubs/{id}");
            if (club == null)
            {
                return NotFound();
            }
            return View(club);
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }

    [Route("Events")]
    public async Task<IActionResult> Events()
    {
        var httpClient = _httpClientFactory.CreateClient("ClubCanvasAPI");
        
        try
        {
            var clubs = await httpClient.GetFromJsonAsync<List<Club>>("clubs");
            return View(clubs ?? new List<Club>());
        }
        catch (HttpRequestException)
        {
            return View(new List<Club>());
        }
    }

    [Route("EventDetails")]
    public async Task<IActionResult> EventDetails(int id)
    {
        var httpClient = _httpClientFactory.CreateClient("ClubCanvasAPI");
        
        try
        {
            // Get all clubs to find the event
            var clubs = await httpClient.GetFromJsonAsync<List<Club>>("clubs");
            
            if (clubs != null)
            {
                foreach (Club c in clubs)
                {
                    if (c.Events != null)
                    {
                        foreach (Event e in c.Events)
                        {
                            if (e.Id == id)
                            {
                                return View(e);
                            }
                        }
                    }
                }
            }
            return NotFound();
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }

    [Route("Canvas")]
    public IActionResult Canvas()
    {
        return View();
    }

    [Route("NewClub")]
    public async Task<IActionResult> NewClub()
    {
        return View();
    }


    [HttpPost]
    [Route("NewClub")]
    public async Task<IActionResult> NewClub(ClubViewModel model)
    {
        if (ModelState.IsValid)
        {
            var newClub = new Club
            {
                Name = model.Name,
                Description = model.Description,
                OwnerId = model.OwnerId,
                Image = model.Image
            };

            await _clubs.AddClubAsync(newClub);
        }

        return View(model);
    }
}