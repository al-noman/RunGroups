namespace RunGroups.Models;

public class EditClubDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile? Image { get; set; }
    public ClubCategory ClubCategory { get; set; }
    public int AddressId { get; set; }
    public Address Address { get; set; }
}