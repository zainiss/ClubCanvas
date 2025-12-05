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


    [Route("Clubs")]
    public async Task<IActionResult> Clubs()
    {
        var httpClient = _httpClientFactory.CreateClient("ClubCanvasAPI");
        
        try
        {
             var clubs = await httpClient.GetFromJsonAsync<List<CreateClubDto>>("clubs");
            
            if (clubs == null)
            {
                return NotFound();
            }

            return View(clubs);
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

    [HttpGet]
    [Route("Clubs/ClubDetails/{id:int}")]
    public async Task<IActionResult> ClubDetails(int id)
    {
        var httpClient = _httpClientFactory.CreateClient("ClubCanvasAPI");
        
        try
        {
            var club = await httpClient.GetFromJsonAsync<CreateClubDto>($"clubs/{id}");

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
            var clubs = await httpClient.GetFromJsonAsync<List<CreateEventDto>>("events");
            return View(clubs ?? new List<CreateEventDto>());
        }
        catch (HttpRequestException)
        {
            return View(new List<CreateEventDto>());
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
    public async Task<IActionResult> Canvas()
    {
        var token = HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Home");
        }

        var httpClient = _httpClientFactory.CreateClient("ClubCanvasAPI");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var owner = await httpClient.GetFromJsonAsync<AuthResponseDto>("auth/me");
        var clubs = await httpClient.GetFromJsonAsync<List<CreateClubDto>>("clubs");
        var events = await httpClient.GetFromJsonAsync<List<CreateEventDto>>("events");

        
        if (owner == null || !owner.Success)
        {
            return RedirectToAction("Login", "Home");
        }

        var userDto = new UserDto
        {
            Username = owner.UserName ?? string.Empty,
            Email = owner.Email ?? string.Empty,
            OwnedClubs = clubs?.Where(c => c.OwnerEmail == owner.Email).ToList() ?? new List<CreateClubDto>(),
        };

        return View(userDto);
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
                var token = HttpContext.Session.GetString("JwtToken");

                if (string.IsNullOrEmpty(token))
                {
                    ModelState.AddModelError("", "Could not authenticate session.");
                    return View(model);
                }

                var httpClient = _httpClientFactory.CreateClient("ClubCanvasAPI");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var owner = await httpClient.GetFromJsonAsync<AuthResponseDto>("auth/me");

                if (owner == null)
                {
                    ModelState.AddModelError(string.Empty, "Please log in to create a club.");
                    return View(model);
                }

                // Create DTO
                var newClubDto = new CreateClubDto
                {
                    Name = model.Name,
                    Description = model.Description,
                    Image = model.Image,
                    OwnerId = owner.UserId,
                    OwnerName = owner.UserName,
                    OwnerEmail = owner.Email,
                    Events = new List<CreateEventDto>(),
                    Members = new List<UserDto>()
                };

                // Make the API call
                var response = await httpClient.PostAsJsonAsync("clubs", newClubDto);

                return RedirectToAction("clubs");
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