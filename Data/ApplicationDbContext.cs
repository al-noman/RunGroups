using Microsoft.EntityFrameworkCore;
using RunGroups.Models;

namespace RunGroups.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<Race> Races { get; set; }
}