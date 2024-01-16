using AutoMapper;
using CarForum.Database;
using CarForum.Extentions;
using CarForum.Models;
using CarForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarForum.Controllers;

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
    [HttpGet("UserProfile")]
    public IActionResult UserProfile()
    {
        var currentUser = _userManager.GetUserAsync(User).Result;
        
        if (currentUser is not null)
            currentUser.Reviews = _context.Reviews.Where(r => r.Author.Email == currentUser.Email).ToList();

        return View(currentUser);
    }

    [Authorize]
    [HttpGet("EditProfile")]
    public IActionResult EditProfile()
    {
        var currentUser = _userManager.GetUserAsync(User).Result;
        return View(
            new EditViewModel
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                Country = currentUser.Country,
                City = currentUser.City,
                DrivingExperienceYears = currentUser.DrivingExperienceYears,
                BirthDay = currentUser.BirthDay
            });
    }

    [HttpPost("Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _mapper.Map<User>(model);
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var fullUser = _userManager.FindByNameAsync(user.UserName).Result;
                return RedirectToAction("UserProfile", "Account", fullUser);
            }
            else
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
        }
        return RedirectToAction("Login", model);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("UserProfile", "Account", user);
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View("Register", model);
    }

    [HttpPost("EditProfile")]
    public async Task<IActionResult> EditProfile(EditViewModel model) 
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(_userManager.GetUserAsync(User).Result.Id);
            user.Convert(model);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return RedirectToAction("UserProfile", "Account");
            else
                return RedirectToAction("EditProfile", "Account");
        }
        else
        {
            ModelState.AddModelError("", "Некорректные данные");
            return View("Edit", model);
        }
    }

    [HttpPost("Logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
}
