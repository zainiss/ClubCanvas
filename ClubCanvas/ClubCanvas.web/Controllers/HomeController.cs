using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ClubCanvas.Core;
using ClubCanvas.Core.Models;
using ClubCanvas.web.Models;

namespace ClubCanvas.web.Controllers;

public class HomeController : Controller
{
    private readonly IUserRepository _users;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public HomeController(IUserRepository users, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _users = users;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [Route("")]
    public async Task<IActionResult> Index()
    {
        Console.WriteLine("Load index");
        
        // Get current user if logged in
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewBag.UserName = user.UserName ?? user.Email;
                return View();
            }
        }
        
        ViewBag.UserName = null;
        return View();
    }

    [HttpGet]
    [Route("Login")]
    public async Task<IActionResult> Login()
    {
        Console.WriteLine("loginh");
        var allUsers = await _users.GetAllUsersAsync();
        foreach(ApplicationUser u in allUsers)
        {
            Console.WriteLine(u.Email);
        }
        return View();
    }


    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Find user by email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                // Verify password and sign in
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
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
    public async Task<IActionResult> Signup(SignupViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email is already registered.");
                return View(model);
            }

            // Create new user with Identity
            var newUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.UserName
            };

            // Create user with password (Identity will hash it)
            var result = await _userManager.CreateAsync(newUser, model.Password);
            
            if (result.Succeeded)
            {
                // Automatically sign in the new user
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                return RedirectToAction("Index");
            }
            else
            {
                // Add errors from Identity
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
