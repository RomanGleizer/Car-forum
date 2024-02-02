using CarForum.Database;
using CarForum.Models;
using CarForum.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CarForum.DAO.ReviewData;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext _context;

    public ReviewRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Review?> GetReviewByIdAsync(int id)
    {
        return await _context.Reviews
            .Include(r => r.Comments)
            .Include(r => r.Author)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<DeletedReviewViewModel?> GetDeletedReviewByIdAsync(int id, string userId)
    {
        return await _context.DeletedReviews
            .Where(r => r.Author.Id == userId && r.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task AddReviewAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
    }

    public async Task EditReviewAsync(Review review)
    {
        _context.Entry(review).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReviewAsync(Review review)
    {
        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }

    public async Task AddDeletedReviewAsync(DeletedReviewViewModel deletedReview)
    {
        _context.DeletedReviews.Add(deletedReview);
        await _context.SaveChangesAsync();
    }

    public async Task RestoreReviewAsync(DeletedReviewViewModel deletedReview)
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

        await _context.SaveChangesAsync();
    }

    public async Task<IQueryable<Review>?> SearchReviewsAsync(FindReviewViewModel reviewModel)
    {
        return _context.Reviews
            .Where(r =>
                r.CarBrand == reviewModel.Brand &&
                r.CarModel == reviewModel.Model &&
                r.Year == reviewModel.Year &&
                r.Transmission == reviewModel.Transmission &&
                r.Body == reviewModel.Body &&
                r.EngineType == reviewModel.EngineType)
            .Include(r => r.Author)
            .Include(r => r.Comments);
    }
}