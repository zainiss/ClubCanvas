using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace ClubCanvas.Shared.DTOs;

public class CreateClubDto
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string? OwnerId { get; set; }
    public string? OwnerEmail { get; set; }
    public string? OwnerName { get; set; }
    
    public string? Image { get; set; }
    
    public List<CreateEventDto>? Events { get; set; }
    public List<UserDto>? Members {get; set;}
}

public class CreateEventDto
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    [Required]
    public DateTime EventDate { get; set; }
    
    public string? Location { get; set; }
    public int? ClubId { get; set; }
}

