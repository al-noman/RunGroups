using Microsoft.AspNetCore.Mvc;
using RunGroups.Repository.Interfaces;

namespace RunGroups.Controllers;

public class ClubController : Controller
{
    private readonly IClubRepository _repository;

    public ClubController(IClubRepository repository)
    {
        _repository = repository;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var clubs = await _repository.GetAllAsync();
        return View(clubs);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var club = await _repository.GetByIdAsync(id);
        return View(club);
    }
}