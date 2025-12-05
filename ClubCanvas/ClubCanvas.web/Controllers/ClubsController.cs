using Microsoft.AspNetCore.Mvc;
using ClubCanvas.Core.Models;
using System.Net.Http.Json;
using ClubCanvas.Core;
using ClubCanvas.web.Models;
using Microsoft.AspNetCore.Identity;
using ClubCanvas.Shared.DTOs;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Net.Http.Headers;

namespace ClubCanvas.web.Controllers;

public class ClubsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly UserManager<ApplicationUser> _userManager;

    public ClubsController(IHttpClientFactory httpClientFactory, UserManager<ApplicationUser> userManager, IClubsRepository clubs)
    {
        _httpClientFactory = httpClientFactory;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("TestConnection")]
    public async Task<IActionResult> TestConnection()
    {
        var results = new List<string>();
        
        try
        {
            var httpClient = _httpClientFactory.CreateClient("ClubCanvasAPI");
            
            // Test 1: Try to reach the API base
            results.Add($"Testing connection to: {httpClient.BaseAddress}");
            
            // Test 2: Try a simple GET
            try
            {
                var response = await httpClient.GetAsync("");
                results.Add($"GET / Response: {(int)response.StatusCode} {response.StatusCode}");
                
                if (response.Content != null)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    results.Add($"GET / Content: {content}");
                }
            }
            catch (Exception ex)
            {
                results.Add($"GET / Failed: {ex.Message}");
            }
            
            // Test 3: Try the clubs endpoint
            try
            {
                var response = await httpClient.GetAsync("clubs");
                results.Add($"GET /clubs Response: {(int)response.StatusCode} {response.StatusCode}");
            }
            catch (Exception ex)
            {
                results.Add($"GET /clubs Failed: {ex.Message}");
            }
            
            return Content(string.Join("\n", results));
        }
        catch (Exception ex)
        {
            return Content($"Setup failed: {ex.Message}\n\nStack: {ex.StackTrace}");
        }
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

    [HttpGet]
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
            try
            {
                // Get user ID (using the correct method)
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = userIdClaim?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    ModelState.AddModelError(string.Empty, "Please log in to create a club.");
                    return View(model);
                }

                var token = HttpContext.Session.GetString("JwtToken");

                if (string.IsNullOrEmpty(token))
                {
                    ModelState.AddModelError("", "You must be logged in to create a club.");
                    return View(model);
                }

                var httpClient = _httpClientFactory.CreateClient("ClubCanvasAPI");
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                
                // Create DTO
                var newClubDto = new CreateClubDto
                {
                    Name = model.Name,
                    Description = model.Description,
                    Image = model.Image,
                    OwnerId = userId,
                    Events = new List<CreateEventDto>()
                };

                // Make the API call
                var response = await httpClient.PostAsJsonAsync("clubs", newClubDto);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"DEBUG: HttpRequestException: {ex.Message}");
                ModelState.AddModelError(string.Empty, 
                    $"Cannot connect to API: {ex.Message}. Make sure the API is running.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: Exception: {ex.Message}");
                Console.WriteLine($"DEBUG: StackTrace: {ex.StackTrace}");
                ModelState.AddModelError(string.Empty, 
                    $"Unexpected error: {ex.Message}");
            }
        }
        
        return View(model);
    }
}