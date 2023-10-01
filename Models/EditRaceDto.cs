namespace RunGroups.Models;

public class EditRaceDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile? Image { get; set; }
    public RaceCategory RaceCategory { get; set; }
    public int AddressId { get; set; }
    public Address Address { get; set; }
}