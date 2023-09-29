using Microsoft.EntityFrameworkCore;
using RunGroups.Data;
using RunGroups.Models;
using RunGroups.Repository.Interfaces;

namespace RunGroups.Repository;

public class ClubRepository : IClubRepository
{
    private readonly ApplicationDbContext _context;

    public ClubRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Club>> GetAllAsync()
    {
        return await _context.Clubs.ToListAsync();
    }

    public async Task<IEnumerable<Club>> GetByCityAsync(string city)
    {
        return await _context.Clubs.Where(club => club.Address.City == city).ToListAsync();
    }

    public async Task<Club?> GetByIdAsync(int id)
    {
        return await _context.Clubs.Include(cl => cl.Address).FirstOrDefaultAsync(cl => cl.Id == id);
    }

    public bool Add(Club club)
    {
        _context.Add(club);
        return Save();
    }

    public bool Update(Club club)
    {
        _context.Update(club);
        return Save();
    }

    public bool Delete(Club club)
    {
        _context.Remove(club);
        return Save();
    }

    public bool Save()
    {
        var result = _context.SaveChanges();
        return result > 0;
    }
}