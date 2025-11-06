namespace ClubCanvas.mvc.Models;

public class Club
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<string> Events { get; set; }
    public List<string> Members { get; set; }
    public string Owner { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
}

