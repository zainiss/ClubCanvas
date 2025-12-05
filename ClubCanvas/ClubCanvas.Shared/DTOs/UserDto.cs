
namespace ClubCanvas.Shared.DTOs;

public class UserDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public List<CreateClubDto>? Clubs { get; set; }
    public List<CreateClubDto>? OwnedClubs { get; set; }
}