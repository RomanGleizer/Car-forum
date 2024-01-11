using Microsoft.AspNetCore.Mvc;

namespace CarForum.Controllers;

[Route("{controller}")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet("Main")]
    public IActionResult Index()
    {
        return View();
    }
}
