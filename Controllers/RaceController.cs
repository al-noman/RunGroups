using Microsoft.AspNetCore.Mvc;
using RunGroups.Repository.Interfaces;

namespace RunGroups.Controllers;

public class RaceController : Controller
{
    private readonly IRaceRepository _repository;
    public RaceController(IRaceRepository repository)
    {
        _repository = repository;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var races = await _repository.GetAllRaceAsync();
        return View(races);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var race = await _repository.GetRaceById(id);
        return View(race);
    }
}