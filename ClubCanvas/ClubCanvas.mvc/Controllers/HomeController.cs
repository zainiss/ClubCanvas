using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClubCanvas.mvc.Models;

namespace ClubCanvas.mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }


    // [HttpPost]
    // [Route("LogIn")]
    // public ViewResult LogIn(User user)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         return View();
    //     }

    //     return View(user);
    // }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
