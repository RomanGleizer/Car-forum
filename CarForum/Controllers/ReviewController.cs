using AutoMapper;
using CarForum.Database;
using CarForum.Extentions;
using CarForum.Models;
using CarForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    [HttpGet("Search")]
    public IActionResult Search() => View();

    [Authorize]
    [HttpGet("EditReview")]
    public async Task<IActionResult> EditReview(int id)
    {
        var review = await GetReviewById(id);
        if (review is null) return NotFound();

        var model = _mapper.Map<EditReviewViewModel>(review);
        return View(model);
    }

    [Authorize]
    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var review = await GetReviewById(id);

        ViewBag.CurrentUser = await _userManager.GetUserAsync(User);
        return ProcessReview(review);
    }

    [Authorize]
    [HttpGet("DeletedReviewDetails/{id}")]
    public async Task<IActionResult> DeletedReviewDetails(int id)
    {
        var review = await GetDeletedReviewById(id);
        return ProcessReview(review);
    }

    [Authorize]
    [HttpGet("DeleteReview/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await GetReviewById(id);
        return ProcessReview(review);
    }

    [Authorize]
    [HttpGet("RestoreReview/{id}")]
    public async Task<IActionResult> Restore(int id)
    {
        var review = await GetDeletedReviewById(id);
        return ProcessReview(review);
    }

    [HttpPost("CreateReview")]
    public async Task<IActionResult> CreateReview(CreateReviewViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is not null)
        {
            var review = _mapper.Map<Review>(model);
            review.AuthorId = currentUser.Id;
            review.Author = currentUser;
            review.PhotoPath = SaveImage(model.Photo);

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("UserProfile", "Account");
        }

        ModelState.AddModelError("", "Пользователя не существует");
        return View(model);
    }

    [HttpPost("EditReview")]
    public async Task<IActionResult> EditReview(EditReviewViewModel model, int id)
    {
        if (!ModelState.IsValid) return View(model);

        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        if (review is not null)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;
            if (currentUser.Id == review.Author.Id)
            {
                review.Convert(model);
                if (model.Photo is not null)
                    review.PhotoPath = SaveImage(model.Photo);

                await _context.SaveChangesAsync();
                return RedirectToAction("UserProfile", "Account");
            }
            else return NotFound();

        }
        
        ModelState.AddModelError("", "Отзыва не существует");
        return View(model);
    }

    [HttpPost("DeleteReview/{id}")]
    public Task<IActionResult> DeleteReview(int id) =>
        ManageReview(id, async (review, isDeleted) =>
        {
            if (!isDeleted)
            {
                var deletedReview = new DeletedReview
                {
                    Title = review.Title,
                    Rating = review.Rating,
                    CarBrand = review.CarBrand,
                    CarModel = review.CarModel,
                    Year = review.Year,
                    Mileage = review.Mileage,
                    Transmission = review.Transmission,
                    Body = review.Body,
                    EngineType = review.EngineType,
                    EngineCapacity = review.EngineCapacity,
                    DriveType = review.DriveType,
                    Advantages = review.Advantages,
                    Disadvantages = review.Disadvantages,
                    OverallExperience = review.OverallExperience,
                    AuthorId = review.AuthorId,
                    Author = review.Author,
                    PhotoPath = review.PhotoPath,
                    PublishTime = review.PublishTime
                };

                _context.DeletedReviews.Add(deletedReview);
            }
            _context.Reviews.Remove(review);
        });

    [HttpPost("RestoreReview/{id}")]
    public Task<IActionResult> RestoreReview(int id) =>
        ManageReview(id, async (review, isDeleted) =>
        {
            if (isDeleted)
            {
                var deletedReview = await _context.DeletedReviews.FindAsync(id);

                if (deletedReview is not null)
                {
                    var restoredReview = new Review
                    {
                        Title = deletedReview.Title,
                        Rating = deletedReview.Rating,
                        CarBrand = deletedReview.CarBrand,
                        CarModel = deletedReview.CarModel,
                        Year = deletedReview.Year,
                        Mileage = deletedReview.Mileage,
                        Transmission = deletedReview.Transmission,
                        Body = deletedReview.Body,
                        EngineType = deletedReview.EngineType,
                        EngineCapacity = deletedReview.EngineCapacity,
                        DriveType = deletedReview.DriveType,
                        Advantages = deletedReview.Advantages,
                        Disadvantages = deletedReview.Disadvantages,
                        OverallExperience = deletedReview.OverallExperience,
                        AuthorId = deletedReview.AuthorId,
                        Author = deletedReview.Author,
                        PhotoPath = deletedReview.PhotoPath,
                        PublishTime = deletedReview.PublishTime
                    };

                    _context.Reviews.Add(restoredReview);
                    _context.DeletedReviews.Remove(deletedReview);
                }
            }
        });

    [HttpPost("Search")]
    public async Task<IActionResult> Search(FindReviewViewModel reviewModel)
    {
        if (!ModelState.IsValid)
            return View(reviewModel);

        var suitableReviews = await _context.Reviews
            .Where(r =>
                r.CarBrand == reviewModel.Brand &&
                r.CarModel == reviewModel.Model &&
                r.Year == reviewModel.Year &&
                r.Transmission == reviewModel.Transmission &&
                r.Body == reviewModel.Body &&
                r.EngineType == reviewModel.EngineType)
            .ToListAsync();

        ViewBag.SuitableReviews = suitableReviews;
        return View("Search");
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

    private async Task<Review?> GetReviewById(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        return await _context.Reviews
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();
    }

    private async Task<DeletedReview?> GetDeletedReviewById(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        return await _context.DeletedReviews
            .Where(r => user.Id == r.Author.Id && r.Id == id)
            .FirstOrDefaultAsync();
    }

    private IActionResult ProcessReview<T>(T review) 
        where T: class
    {
        if (review is null) return NotFound();
        return View(review);
    }

    private async Task<IActionResult> ManageReview(int id, Func<Review, bool, Task> action)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is null)
        {
            ModelState.AddModelError("", "Такого пользователя нет");
            return NotFound();
        }

        var review = await _context.Reviews.FindAsync(id);
        bool isDeleted = false;

        if (review is null)
        {
            var deletedReview = await _context.DeletedReviews.FindAsync(id);
            if (deletedReview is null)
            {
                ModelState.AddModelError("", "Отзыва не существует");
                return NotFound();
            }

            review = _mapper.Map<Review>(deletedReview);
            isDeleted = true;
        }

        await action(review, isDeleted);
        await _context.SaveChangesAsync();
        return RedirectToAction("UserProfile", "Account");
    }

}
