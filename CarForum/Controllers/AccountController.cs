using AutoMapper;
using CarForum.Models;
using CarForum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarForum.Controllers;

[Route("api/{controller}")]
public class AccountController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager) : Controller
{
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;

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

    [HttpPost("Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _mapper.Map<User>(model);
            var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded) 
                return RedirectToAction("Личная страница пользователя", "Account");
            else
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
        }
        return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Личная страница пользователя", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return RedirectToAction("Index", "Home");
    }
}
