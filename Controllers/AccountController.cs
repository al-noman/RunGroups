using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroups.Models;

namespace RunGroups.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login()
    {
        var loginDto = new LoginDto();
        return View(loginDto);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid) return View(loginDto);

        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            TempData["Error"] = "Incorrect email or password provided";
            return View(loginDto);
        }

        var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!passwordCheck)
        {
            TempData["Error"] = "Incorrect email or password provided";
            return View(loginDto);
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
        if (!result.Succeeded)
        {
            TempData["Error"] = "Login failed";
            return View(loginDto);
        }

        return RedirectToAction("Index", "Race");
    }

    public IActionResult Register()
    {
        var registerDto = new RegisterDto();
        return View(registerDto);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid) return View(registerDto);
        var user = await _userManager.FindByEmailAsync(registerDto.Email);
        if (user != null)
        {
            TempData["Error"] = "User already exist with this email address";
            return View(registerDto);
        }

        var newAppUser = new AppUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Email
        };
        var userSavingResponse = await _userManager.CreateAsync(newAppUser, registerDto.Password);

        if (!userSavingResponse.Succeeded)
        {
            TempData["Error"] = "Could not create a new User due to internal error";
            return View(registerDto);
        }

        await _userManager.AddToRoleAsync(newAppUser, UserRoles.User);
        
        return RedirectToAction("Index", "Race");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Race");
    }
}
