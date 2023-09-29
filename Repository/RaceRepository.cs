using Microsoft.EntityFrameworkCore;
using RunGroups.Data;
using RunGroups.Models;
using RunGroups.Repository.Interfaces;

namespace RunGroups.Repository;

public class RaceRepository : IRaceRepository
{
    private readonly ApplicationDbContext _context;

    public RaceRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Race>> GetAllRaceAsync()
    {
        return await _context.Races.ToListAsync();
    }

    public async Task<IEnumerable<Race>> GetRacesByCity(string city)
    {
        return await _context.Races.Where(rc => rc.Address.City == city).ToListAsync();
    }

    public async Task<Race?> GetRaceById(int id)
    {
        return await _context.Races.Include(rc => rc.Address).FirstOrDefaultAsync(rc => rc.Id == id);
    }

    public bool Add(Race race)
    {
        _context.Add(race);
        return Save();
    }

    public bool Update(Race race)
    {
        _context.Update(race);
        return Save();
    }

    public bool Delete(Race race)
    {
        _context.Remove(race);
        return Save();
    }

    public bool Save()
    {
        var result = _context.SaveChanges();
        return result > 0;
    }
}