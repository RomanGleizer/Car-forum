using CarForum.Database;
using CarForum.Models;
using CarForum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarForum.Controllers;

[ApiController]
[Route("api/{controller}")]
public class CommentController(ApplicationDbContext context, UserManager<User> userManager) : Controller
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;

    [HttpPost("AddComment")]
    public async Task<IActionResult> AddComment([FromForm] CreateCommentViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is null) return NotFound("Пользователь не найден");

        var review = await _context.Reviews
            .Include(r => r.Comments)
            .FirstOrDefaultAsync(r => r.Id == model.Id);

        if (review is null)
            return NotFound();

        var comment = new Comment
        {
            Text = model.Text,
            Review = review,
            ReviewId = review.Id,
            Author = currentUser,
            PublishTime = DateTime.Now
        };

        review.Comments.Add(comment);
        await _context.SaveChangesAsync();

        var updatedReview = await _context.Reviews
               .Include(r => r.Comments)
               .Include(r => r.Author)
               .FirstOrDefaultAsync(r => r.Id == model.Id);

        return PartialView("_CommentListPartial", Tuple.Create((IEnumerable<Comment>)review.Comments, currentUser, updatedReview));
    }

    [HttpPost("LikeComment/{commentId}")]
    public async Task<IActionResult> LikeComment(int commentId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is null) return NotFound();

        var comment = await GetCommentByIdAsync(commentId);
        if (comment is null) return NotFound();

        var userId = currentUser.Id;
        var isLikedByCurrentUser = comment.LikedByUserIds.Contains(userId);

        if (!isLikedByCurrentUser)
        {
            comment.LikedByUserIds.Add(userId);
            comment.LikesAmount++;
        }
        else
        {
            comment.LikedByUserIds.Remove(userId);
            comment.LikesAmount--;
        }

        await _context.SaveChangesAsync();

        return Json(new { likesAmount = comment.LikesAmount, isLikedByCurrentUser = !isLikedByCurrentUser });
    }


    [HttpPut("EditComment/{commentId}")]
    public async Task<IActionResult> EditComment(int commentId, [FromBody] EditCommentViewModel model)
    {
        var comment = await GetCommentByIdAsync(commentId);
        if (comment is null) return NotFound();

        comment.Text = model.Text;
        await _context.SaveChangesAsync();

        return Json(comment);
    }

    [HttpDelete("DeleteComment/{commentId}")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var comment = await GetCommentByIdAsync(commentId);
        if (comment is null) return NotFound();

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return Ok();
    }

    private async Task<Comment> GetCommentByIdAsync(int id)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == id);
        return comment;
    }
}