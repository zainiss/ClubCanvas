using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClubCanvas.mvc.Models;
using ClubCanvas.mvc.Database;

namespace ClubCanvas.mvc.Controllers;

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
        foreach(User u in _users.GetAllUsers())
        {
            Console.WriteLine(u.Email);
        }
        return View();
    }


    [HttpPost]
    [Route("Login")]
    public IActionResult Login(User user)
    {
        if (ModelState.IsValid)
        {
            User targetLogin = _users.GetUserByEmail(user.Email);
            if (targetLogin != null)
            {
                if (targetLogin.Password == user.Password)
                {
                    return RedirectToAction("Clubs", "Clubs");
                }
            }
        }

        return View(user);
    }
    
    [HttpGet]
    [Route("Signup")]
    public IActionResult Signup()
    {
        return View();
    }

    [HttpPost]
    [Route("Signup")]
    public IActionResult Signup(User user)
    {
        if (ModelState.IsValid)
        {
            List<User> allUsers = _users.GetAllUsers();

            bool emailUsed = false;
            foreach (User u in allUsers)
            {
                if (user.Email == u.Email)
                {
                    emailUsed = true;
                    break;
                }
            }
            
            if (emailUsed == false)
            {
                _users.AddUser(user);
                foreach(User u in _users.GetAllUsers())
                {
                    Console.WriteLine(u.Email);
                }
                return RedirectToAction("Index");
            }
        }

        return View(user);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
