using CarForum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Emit;
using CarForum.ViewModels;

namespace CarForum.Database;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public override DbSet<User> Users { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<DeletedReviewViewModel> DeletedReviews { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Comment>()
            .HasOne(c => c.Review)
            .WithMany(r => r.Comments)
            .HasForeignKey(c => c.ReviewId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<DeletedReviewViewModel>()
            .Property(r => r.Id)
            .ValueGeneratedOnAdd();

        base.OnModelCreating(builder);
    }
}
