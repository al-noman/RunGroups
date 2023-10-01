using RunGroups.Models;

namespace RunGroups.Repository.Interfaces;

public interface IClubRepository
{
    Task<IEnumerable<Club>> GetAllAsync();
    Task<IEnumerable<Club>> GetByCityAsync(string city);
    Task<Club?> GetByIdAsync(int id);
    Task<Club?> GetByIdNoTrackingAsync(int id);
    bool Add(Club club);
    bool Update(Club club);
    bool Delete(Club club);
    bool Save();
}