using Microsoft.AspNetCore.Identity;

namespace ClubCanvas.Core.Models;

public class ApplicationUser : IdentityUser
{
    // IdentityUser already provides:
    // - Id (string)
    // - UserName (string)
    // - Email (string)
    // - PasswordHash (string)
    // - And many other properties
    
    // Add any additional properties specific to your application here
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public ICollection<Club>? OwnedClubs { get; set; }
}

