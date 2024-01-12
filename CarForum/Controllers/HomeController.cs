using Microsoft.AspNetCore.Mvc;

namespace CarForum.Controllers;

[ApiController]
[Route("")]
[Route("api/[controller]")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
