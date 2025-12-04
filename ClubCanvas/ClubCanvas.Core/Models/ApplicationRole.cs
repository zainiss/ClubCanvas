using Microsoft.AspNetCore.Identity;

namespace ClubCanvas.Core.Models;

public class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }
}

