using System.ComponentModel.DataAnnotations;

namespace ClubCanvas.API.Models;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(3)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    [Required]
    public string UserName { get; set; } = string.Empty;
}

