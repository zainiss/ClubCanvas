using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ClubCanvas.Core;
using ClubCanvas.Core.Models;
using ClubCanvas.Infrastructure.Repositories;
using ClubCanvas.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure HttpClient to call the API
builder.Services.AddHttpClient("ClubCanvasAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5076/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false; // Changed to false - special characters not required
    options.Password.RequiredLength = 3;
    
    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClubsRepository, ClubsRepository>();
builder.Services.AddScoped<IEventsRepository, EventsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.UseSession();
app.Run();
