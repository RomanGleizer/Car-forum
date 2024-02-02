using AutoMapper;
using CarForum.DAO.AccountData;
using CarForum.Database;
using CarForum.Models;
using CarForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CarForum.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : Controller
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public AccountController(IAccountRepository accountRepository, IMapper mapper, ApplicationDbContext context)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
        _context = context;
    }

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
        ViewBag.Reviews = _context.Reviews
            .Include(c => c.Author)
            .Include(c => c.Comments).ToList();

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
        var currentUser = await _accountRepository.GetUserAsync(User.Identity.Name);

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
        var currentUser = await _accountRepository.GetUserAsync(User.Identity.Name);
        var model = _mapper.Map<EditProfileViewModel>(currentUser);

        return View(model);
    }

    [HttpPost("Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _accountRepository.SignInAsync(model.UserName, model.Password, model.RememberMe);

        if (result)
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

        var result = await _accountRepository.CreateUserAsync(user, model.Password);

        if (result)
        {
            await _accountRepository.SignInAsync(user.UserName, model.Password, rememberMe: false);
            return RedirectToAction("UserProfile", "Account");
        }

        return View(model);
    }

    [HttpPost("EditProfile")]
    public async Task<IActionResult> EditProfile([FromForm] EditProfileViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var currentUser = await _accountRepository.GetUserAsync(User.Identity.Name);
        if (currentUser is null) return NotFound();

        _mapper.Map(model, currentUser);
        var result = await _accountRepository.UpdateUserAsync(currentUser);

        if (result)
            return RedirectToAction("UserProfile", "Account");

        return View(model);
    }

    [HttpPost("Logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _accountRepository.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [HttpPost("ResetPassword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _accountRepository.GetUserByEmailAsync(model.Email);

        if (user is null)
            return View(model);

        var result = await _accountRepository.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

        if (result)
            return RedirectToAction("Login", "Account");

        return View(model);
    }
}