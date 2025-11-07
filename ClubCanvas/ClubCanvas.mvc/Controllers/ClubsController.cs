using Microsoft.AspNetCore.Mvc;
using ClubCanvas.mvc.Database;
using System.Reflection.Metadata.Ecma335;
using ClubCanvas.mvc.Models;

namespace Clubs.mvc.Controllers;

public class ClubsController : Controller
{   private readonly IClubsRepository _clubs;

    public ClubsController(IClubsRepository clubs)
    {
        _clubs = clubs;
    }

    [Route("Clubs")]
    public IActionResult Clubs()
    {
        var clubs = _clubs.GetAllClubs();
        return View(clubs);
    }

    public IActionResult ClubDetails(int id)
    {
        var club = _clubs.GetClubById(id);
        if (club == null)
        {
            return NotFound();
        }
        return View(club);
    }

    [Route("Events")]
    public IActionResult Events()
    {
        var clubs = _clubs.GetAllClubs();
        return View(clubs);
    }

    [Route("EventDetails")]
    public IActionResult EventDetails(int id)
    {
        foreach (Club c in _clubs.GetAllClubs())
        {
            foreach (Event e in c.Events)
            {
                if (e.Id == id)
                {
                    return View(e);
                }
            }
        }
        return NotFound();
    }


    [Route("Canvas")]
    public IActionResult Canvas()
    {
        return View();
    }
}