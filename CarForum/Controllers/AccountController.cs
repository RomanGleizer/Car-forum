using AutoMapper;
using CarForum.Database;
using CarForum.Models;
using CarForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CarForum.Controllers;

[ApiController]
[Route("api/{controller}")]
public class AccountController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context) : Controller
{
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly ApplicationDbContext _context = context;

    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet("Register")]
    public IActionResult Register()
    {
        return View();
    }

    [Authorize]
    [HttpGet("Favorite")]
    public IActionResult Favorite()
    {
        return View();
    }

    [HttpGet("ResetPassword")]
    public IActionResult ResetPassword()
    {
        return View();
    }

    [Authorize]
    [HttpGet("UserProfile")]
    public async Task<IActionResult> UserProfile()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser is not null)
        {
            currentUser.DeletedReviews = _context.DeletedReviews.Where(r => r.Author.Id == currentUser.Id).ToList();
            currentUser.Reviews = _context.Reviews.Where(r => r.Author.Id == currentUser.Id).ToList();
        }

        return View(currentUser);
    }

    [Authorize]
    [HttpGet("EditProfile")]
    public async Task<IActionResult> EditProfile()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var model = _mapper.Map<EditProfileViewModel>(currentUser);

        return View(model);
    }

    [HttpPost("Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = _mapper.Map<User>(model);
        var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

        if (result.Succeeded)
            return RedirectToAction("UserProfile", "Account");

        return View(model);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = _mapper.Map<User>(model);

        var russianCulture = new CultureInfo("ru-RU");
        int monthValue = DateTime.ParseExact(model.Month, "MMMM", russianCulture).Month;
        user.BirthDay = new DateTime(model.Year, monthValue, model.Day);

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("UserProfile", "Account");
        }
        
        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }

    [HttpPost("EditProfile")]
    public async Task<IActionResult> EditProfile([FromForm] EditProfileViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user is null) return NotFound();

        _mapper.Map(model, user);
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
            return RedirectToAction("UserProfile", "Account");
        
        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }

    [HttpPost("Logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [HttpPost("ResetPassword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
            return View(model);

        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

        if (result.Succeeded)
            return View("Login", "Account");

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }
}