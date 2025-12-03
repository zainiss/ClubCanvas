using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClubCanvas.Core;
using ClubCanvas.Core.Models;
using ClubCanvas.web.Models;

namespace ClubCanvas.web.Controllers;

public class HomeController : Controller
{
    private readonly IUserRepository _users;

    public HomeController(IUserRepository users)
    {
        _users = users;
    }

    [Route("")]
    public IActionResult Index()
    {
        Console.WriteLine("Load index");
        return View();
    }

    [HttpGet]
    [Route("Login")]
    public IActionResult Login()
    {
        Console.WriteLine("loginh");
        foreach(ApplicationUser u in _users.GetAllUsers())
        {
            Console.WriteLine(u.Email);
        }
        return View();
    }


    [HttpPost]
    [Route("Login")]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser targetLogin = _users.GetUserByEmail(model.Email);
            if (targetLogin != null)
            {
                // TODO: Replace with Identity password verification when Identity is fully integrated
                // For now, this is a placeholder - Identity will handle password verification
                return RedirectToAction("Clubs", "Clubs");
            }
        }

        return View(model);
    }
    
    [HttpGet]
    [Route("Signup")]
    public IActionResult Signup()
    {
        return View();
    }

    [HttpPost]
    [Route("Signup")]
    public IActionResult Signup(SignupViewModel model)
    {
        if (ModelState.IsValid)
        {
            List<ApplicationUser> allUsers = _users.GetAllUsers();

            bool emailUsed = false;
            foreach (ApplicationUser u in allUsers)
            {
                if (model.Email == u.Email)
                {
                    emailUsed = true;
                    break;
                }
            }
            
            if (emailUsed == false)
            {
                var newUser = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.UserName
                    // Password will be handled by Identity UserManager
                };
                _users.AddUser(newUser);
                foreach(ApplicationUser u in _users.GetAllUsers())
                {
                    Console.WriteLine(u.Email);
                }
                return RedirectToAction("Index");
            }
        }

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
