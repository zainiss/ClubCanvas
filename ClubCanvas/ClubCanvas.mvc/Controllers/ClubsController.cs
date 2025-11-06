using Microsoft.AspNetCore.Mvc;
using ClubCanvas.mvc.Database;

namespace ClubCanvas.mvc.Controllers;

public class ClubsController : Controller
{
    private readonly IClubsRepository _clubs;

    public ClubsController(IClubsRepository clubs)
    {
        _clubs = clubs;
    }

    public IActionResult Index()
    {
        var clubs = _clubs.GetAllClubs();
        return View(clubs);
    }

    public IActionResult Details(int id)
    {
        var club = _clubs.GetClubById(id);
        if (club == null)
        {
            return NotFound();
        }
        return View(club);
    }
}