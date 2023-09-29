using RunGroups.Models;

namespace RunGroups.Repository.Interfaces;

public interface IRaceRepository
{
    Task<IEnumerable<Race>> GetAllRaceAsync();
    Task<IEnumerable<Race>> GetRacesByCity(string city);
    Task<Race?> GetRaceById(int id);
    bool Add(Race race);
    bool Update(Race race);
    bool Delete(Race race);
    bool Save();
}