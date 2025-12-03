using Microsoft.EntityFrameworkCore;
using ClubCanvas.Core;
using ClubCanvas.Infrastructure.Repositories;
using ClubCanvas.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure HttpClient to call the API
builder.Services.AddHttpClient("ClubCanvasAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5076/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Temporary: Keep UserRepository for HomeController until we add Identity
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClubsRepository, ClubsRepository>();
//builder.Services.AddScoped<IEventsRepository, EventsRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
