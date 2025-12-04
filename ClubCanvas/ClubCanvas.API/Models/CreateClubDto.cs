using System.ComponentModel.DataAnnotations;

namespace ClubCanvas.API.Models;

public class CreateClubDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public string? Image { get; set; }
    
    public List<CreateEventDto>? Events { get; set; }
}

public class CreateEventDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    [Required]
    public DateTime EventDate { get; set; }
    
    public string? Location { get; set; }
}

