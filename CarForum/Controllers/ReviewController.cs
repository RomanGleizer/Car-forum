using AutoMapper;
using CarForum.DAO.ReviewData;
using CarForum.Database;
using CarForum.Extentions;
using CarForum.Models;
using CarForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarForum.Controllers;

[ApiController]
[Route("api/{controller}")]
public class ReviewController(
    IMapper mapper, 
    ApplicationDbContext context, 
    UserManager<User> userManager, 
    IWebHostEnvironment hostEnvironment,
    IReviewRepository reviewRepository) : Controller
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IWebHostEnvironment _webHostEnvironment = hostEnvironment;
    private readonly IReviewRepository _reviewRepository = reviewRepository;

    [Authorize]
    [HttpGet("CreateReview")]
    public IActionResult CreateReview() => View();

    [HttpGet("Search")]
    public IActionResult Search() => View();

    [Authorize]
    [HttpGet("EditReview")]
    public async Task<IActionResult> EditReview(int id)
    {
        var review = await _reviewRepository.GetReviewByIdAsync(id);
        if (review is null) return NotFound("Отзыв не найден");

        var model = _mapper.Map<EditReviewViewModel>(review);
        return View(model);
    }

    [Authorize]
    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var review = await _reviewRepository.GetReviewByIdAsync(id);

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
        var review = await _reviewRepository.GetReviewByIdAsync(id);
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
    public async Task<IActionResult> CreateReview([FromForm] CreateReviewViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is not null)
        {
            var review = _mapper.Map<Review>(model);
            review.AuthorId = currentUser.Id;
            review.Author = currentUser;
            review.PhotoPath = SaveImage(model.Photo);

            await _reviewRepository.AddReviewAsync(review);
            return RedirectToAction("UserProfile", "Account");
        }

        ModelState.AddModelError("", "Пользователя не существует");
        return View(model);
    }

    [HttpPost("EditReview")]
    public async Task<IActionResult> EditReview([FromForm] EditReviewViewModel model, int id)
    {
        if (!ModelState.IsValid) return View(model);

        var review = await _reviewRepository.GetReviewByIdAsync(id);
        if (review is not null)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null)
                return NotFound("Пользователь не найден");

            if (currentUser.Id == review.Author.Id)
            {
                review.Convert(model);
                if (model.Photo is not null)
                    review.PhotoPath = SaveImage(model.Photo);

                await _reviewRepository.EditReviewAsync(review);
                return RedirectToAction("UserProfile", "Account");
            }
        }

        ModelState.AddModelError("", "Отзыва не существует");
        return View(model);
    }

    [HttpPost("DeleteReview/{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var review = await _reviewRepository.GetReviewByIdAsync(id);
        return await ManageReview(id, async (r, isDeleted) =>
        {
            if (!isDeleted)
            {
                var deletedReview = new DeletedReviewViewModel
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

                await _reviewRepository.AddDeletedReviewAsync(deletedReview);
            }
            await _reviewRepository.DeleteReviewAsync(r);
        });
    }

    [HttpPost("RestoreReview/{id}")]
    public async Task<IActionResult> RestoreReview(int id)
    {
        return await ManageReview(id, async (review, isDeleted) =>
        {
            if (isDeleted)
            {
                var deletedReview = await _reviewRepository.GetDeletedReviewByIdAsync(id, review.Author.Id);

                if (deletedReview is not null)
                    await _reviewRepository.RestoreReviewAsync(deletedReview);
            }
        });
    }

    [HttpPost("Search")]
    public async Task<IActionResult> Search([FromForm] FindReviewViewModel reviewModel)
    {
        if (!ModelState.IsValid)
            return View(reviewModel);

        var suitableReviews = await _reviewRepository.SearchReviewsAsync(reviewModel);

        ViewBag.SuitableReviews = suitableReviews;
        return View("Search");
    }

    [HttpPost("AddInFavorite/{id}")]
    public async Task<IActionResult> AddInFavorite(int id)
    {
        var review = await _reviewRepository.GetReviewByIdAsync(id);
        if (review is null) return NotFound(review);

        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is null) return NotFound(currentUser);

        if (!currentUser.FavoriteReviewByIds.Contains(review.Id))
        {
            currentUser.FavoriteReviewByIds.Add(review.Id);
            await _userManager.UpdateAsync(currentUser);
            return Json(new { isAddedInUserFavorites = true });
        }

        currentUser.FavoriteReviewByIds.Remove(review.Id);
        await _userManager.UpdateAsync(currentUser);
        return Json(new { isAddedInUserFavorites = false });
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

    private async Task<DeletedReviewViewModel?> GetDeletedReviewById(int id)
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

        var review = await _reviewRepository.GetReviewByIdAsync(id);
        bool isDeleted = false;

        if (review is null)
        {
            var deletedReview = await _reviewRepository.GetDeletedReviewByIdAsync(id, currentUser.Id);
            if (deletedReview is null)
            {
                ModelState.AddModelError("", "Отзыва не существует");
                return NotFound();
            }

            review = _mapper.Map<Review>(deletedReview);
            isDeleted = true;
        }

        await action(review, isDeleted);
        return RedirectToAction("UserProfile", "Account");
    }
}