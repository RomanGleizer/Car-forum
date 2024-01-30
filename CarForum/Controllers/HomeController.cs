using CarForum.Database;
using CarForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarForum.Controllers;

[ApiController]
[Route("")]
[Route("api/[controller]")]
public class HomeController(ApplicationDbContext context, UserManager<User> userManager) : Controller
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null || !User.Identity.IsAuthenticated)
            return RedirectToAction("Login", "Account");

        var randomReview = _context.Reviews
                .Include(r => r.Author)
                .Include(r => r.Comments)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.Author)
                .OrderBy(r => Guid.NewGuid())
                .Take(1)
                .SingleOrDefault();

        ViewBag.RandomReview = randomReview;
        ViewBag.CurrentUser = user;
        return View();
    }
}
