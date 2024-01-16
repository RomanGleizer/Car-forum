using AutoMapper;
using CarForum.Database;
using CarForum.Models;
using CarForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarForum.Controllers;

[Route("api/{controller}")]
public class ReviewController(IMapper mapper, ApplicationDbContext context, UserManager<User> userManager, IWebHostEnvironment hostEnvironment) : Controller
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IWebHostEnvironment _webHostEnvironment = hostEnvironment;

    [Authorize]
    [HttpGet("CreateReview")]
    public IActionResult CreateReview() => View();


    [HttpPost("CreateReview")]
    public async Task<IActionResult> CreateReview(CreateReviewViewModel model)
    {
        if (ModelState.IsValid)
        {
            var review = _mapper.Map<Review>(model);
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser is not null)
            {
                review.Author = currentUser;
                review.PhotoPath = SaveImage(model.Photo);

                currentUser.Reviews.Add(review);

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                await _userManager.UpdateAsync(currentUser);

                return RedirectToAction("UserProfile", "Account");
            }
            else
            {
                ModelState.AddModelError("", "Пользователя не существует");
            }
        }
        else
        {
            ModelState.AddModelError("", "Ошибка при заполнении данных");
        }

        return View();
    }

    private string SaveImage(IFormFile photo)
    {
        if (photo != null && photo.Length > 0)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
                photo.CopyTo(fileStream);

            return Path.Combine("/uploads", uniqueFileName);
        }

        return null;
    }
}
