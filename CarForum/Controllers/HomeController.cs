using CarForum.Database;
using CarForum.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarForum.Controllers;

[ApiController]
[Route("")]
[Route("api/[controller]")]
public class HomeController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    public IActionResult Index()
    {
        var rnd = new Random();
        var allReviews = _context.Reviews.ToList();
        var maxReviewsToShow = 1;
        var randomReviews = allReviews.OrderBy(x => rnd.Next()).Take(maxReviewsToShow).ToList();
        ViewBag.RandomReviews = randomReviews;

        return View();
    }
}
