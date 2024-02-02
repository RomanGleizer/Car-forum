using CarForum.Models;
using CarForum.ViewModels;

namespace CarForum.DAO.ReviewData;

public interface IReviewRepository
{
    Task<Review> GetReviewByIdAsync(int id);
    Task<DeletedReviewViewModel> GetDeletedReviewByIdAsync(int id, string userId);
    Task AddReviewAsync(Review review);
    Task EditReviewAsync(Review review);
    Task DeleteReviewAsync(Review review);
    Task AddDeletedReviewAsync(DeletedReviewViewModel deletedReview);
    Task RestoreReviewAsync(DeletedReviewViewModel deletedReview);
    Task<IQueryable<Review>> SearchReviewsAsync(FindReviewViewModel reviewModel);
}