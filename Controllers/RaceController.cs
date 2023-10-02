using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroups.Models;
using RunGroups.Repository.Interfaces;
using RunGroups.Services.Interfaces;

namespace RunGroups.Controllers;

public class RaceController : Controller
{
    private readonly IRaceRepository _repository;
    private readonly IPhotoService _photoService;

    public RaceController(IRaceRepository repository, IPhotoService photoService)
    {
        _repository = repository;
        _photoService = photoService;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var races = await _repository.GetAllRaceAsync();
        return View(races);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var race = await _repository.GetRaceByIdAsync(id);
        return View(race);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RaceDto raceDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("Image", "Photo upload failed!");
            return View(raceDto);
        }

        var uploadResult = await _photoService.AddPhotoAsync(raceDto.Image);
        var race = ConvertRaceDtoToModel(dto: raceDto, imageUrl: uploadResult.Url.ToString());
        _repository.Add(race);
        return RedirectToAction("Index");
    }

    private Race ConvertRaceDtoToModel(RaceDto dto, string imageUrl)
    {
        return new Race
        {
            Title = dto.Title,
            Description = dto.Description,
            RaceCategory = dto.RaceCategory,
            Image = imageUrl,
            Address = new Address()
            {
                Street = dto.Address.Street,
                City = dto.Address.City,
                State = dto.Address.State
            }
        };
    }

    public async Task<IActionResult> Edit(int id)
    {
        var race = await _repository.GetRaceByIdAsync(id);
        if (race == null) return View();

        var raceDto = ConvertRaceToEditRaceDto(race);
        return View(raceDto);
    }

    private EditRaceDto ConvertRaceToEditRaceDto(Race race)
    {
        return new EditRaceDto
        {
            Title = race.Title,
            Description = race.Description,
            RaceCategory = race.RaceCategory,
            AddressId = race.AddressId,
            Address = race.Address
        };
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditRaceDto editRaceDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(ModelState.Keys.First(), "Invalid form data");
            return View(editRaceDto);
        }

        var persistedRace = await _repository.GetRaceByIdNoTrackingAsync(id);
        if (persistedRace == null) return View(editRaceDto);

        var uploadResult = new ImageUploadResult();
        if (persistedRace.Image != null) await _photoService.DeletePhotoAsync(persistedRace.Image);
        if (editRaceDto.Image != null) uploadResult = await _photoService.AddPhotoAsync(editRaceDto.Image);
        
        var race = UpdateRaceProperties(id, editRaceDto, uploadResult.Url.ToString());

        _repository.Update(race);

        return RedirectToAction("Index");
    }

    private Race UpdateRaceProperties(int id, EditRaceDto editRaceDto, string imageUrl)
    {
        return new Race
        {
            Id = id,
            Title = editRaceDto.Title,
            Description = editRaceDto.Description,
            RaceCategory = editRaceDto.RaceCategory,
            Image = imageUrl,
            AddressId = editRaceDto.AddressId,
            Address = editRaceDto.Address
        };
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var race = await _repository.GetRaceByIdAsync(id);
        if (race == null)
        {
            var errorModel = new ErrorViewModel { RequestId = id.ToString() };
            return View("Error", errorModel);
        }

        _repository.Delete(race);
        return RedirectToAction("Index");
    }
}