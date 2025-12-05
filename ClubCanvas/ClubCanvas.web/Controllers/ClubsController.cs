using Microsoft.AspNetCore.Mvc;
using ClubCanvas.Core.Models;
using System.Net.Http.Json;
using ClubCanvas.Core;
using ClubCanvas.web.Models;
using Microsoft.AspNetCore.Identity;

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


//     [HttpPost]
//     [Route("NewClub")]
//     public async Task<IActionResult> NewClub(ClubViewModel model)
//     {
//     //     if (ModelState.IsValid)
//     //     {
//     //         var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
//     //         var userObject = _userManager.GetUserAsync(User).Result;

//     //         if (userId == null || userObject == null)
//     //         {
//     //             ModelState.AddModelError(string.Empty, "Could not get active user");
//     //             return View(model);
//     //         }

//     //         var newClubDto = new CreateClubDto
//     //         {
//     //             Name = model.Name,
//     //             Description = model.Description,
//     //             OwnerId = userId,
//     //             Owner = userObject,
//     //             Image = model.Image
//     //         };

//     //         try
//     //         {
//     //             var httpClient = _httpClientFactory.CreateClient("ClubCanvasAPI");
//     //             var response = await httpClient.PostAsJsonAsync("clubs", newClubDto);

//     //             if (response.IsSuccessStatusCode)
//     //             {
//     //                 // Redirect to list of clubs after successful creation
//     //                 return RedirectToAction("Clubs");
//     //             }
//     //             else
//     //             {
//     //                 ModelState.AddModelError(string.Empty, "Failed to create club via API.");
//     //             }
//     //         }
//     //         catch (HttpRequestException ex)
//     //         {
//     //             ModelState.AddModelError(string.Empty, ex.Message);
//     //         }
//     //     }

//     //     return View(model);
//     // }
}