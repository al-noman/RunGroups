using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroups.Models;
using RunGroups.Repository.Interfaces;
using RunGroups.Services.Interfaces;

namespace RunGroups.Controllers;

public class ClubController : Controller
{
    private readonly IClubRepository _repository;
    private readonly IPhotoService _photoService;

    public ClubController(IClubRepository repository, IPhotoService photoService)
    {
        _repository = repository;
        _photoService = photoService;
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

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateClubDto createClubDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("Image", "Photo upload failed");
            return View(createClubDto);
        }

        var uploadResult = await _photoService.AddPhotoAsync(createClubDto.Image);
        var club = ConvertClubDtoToModel(createClubDto: createClubDto, imageUrl: uploadResult.Url.ToString());
        _repository.Add(club);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var club = await _repository.GetByIdAsync(id);
        if (club == null) return View();
        var editClubDto = ConvertClubToEditClubDto(club);
        return View(editClubDto);
    }

    private EditClubDto ConvertClubToEditClubDto(Club club)
    {
        return new EditClubDto
        {
            Title = club.Title,
            Description = club.Description,
            AddressId = club.AddressId,
            Address = club.Address,
            ClubCategory = club.ClubCategory,
        };
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditClubDto editClubDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("Image", "Failed to upload new picture");
            return View(editClubDto);
        }
        
        var persistedClub = await _repository.GetByIdNoTrackingAsync(id);
        if (persistedClub == null) return View(editClubDto);
        
        var imageUploadResult = new ImageUploadResult();
        await _photoService.DeletePhotoAsync(persistedClub.Image);
        if (editClubDto.Image != null) imageUploadResult = await _photoService.AddPhotoAsync(editClubDto.Image);

        var club = UpdateClubProperties(id: id, editClubDto: editClubDto, imageUrl: imageUploadResult.Url.ToString());

        _repository.Update(club);
        return RedirectToAction("Index");
    }

    private Club UpdateClubProperties(int id, EditClubDto editClubDto, string imageUrl)
    {
        return new Club
        {
            Id = id,
            Title = editClubDto.Title,
            Description = editClubDto.Description,
            ClubCategory = editClubDto.ClubCategory,
            AddressId = editClubDto.AddressId,
            Address = editClubDto.Address,
            Image = imageUrl
        };
    }

    private Club ConvertClubDtoToModel(CreateClubDto createClubDto, string imageUrl)
    {
        return new Club
        {
            Title = createClubDto.Title,
            Description = createClubDto.Description,
            ClubCategory = createClubDto.ClubCategory,
            Image = imageUrl,
            Address = new Address
            {
                Street = createClubDto.Address.Street,
                City = createClubDto.Address.City,
                State = createClubDto.Address.State
            }
        };
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var club = await _repository.GetByIdAsync(id);
        if (club == null)
        {
            var errorModel = new ErrorViewModel { RequestId = id.ToString() };
            return View("Error", errorModel);
        }

        _repository.Delete(club);
        return RedirectToAction("Index");
    }
}