namespace RunGroups.Models;

public class CreateClubDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile Image { get; set; }
    public ClubCategory ClubCategory { get; set; }
    public Address Address { get; set; }
}