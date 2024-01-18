using CarForum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CarForum.Database;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public override DbSet<User> Users { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<DeletedReview> DeletedReviews { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Review>()
            .HasOne(r => r.Author)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.AuthorId);

        builder.Entity<DeletedReview>()
            .Property(r => r.Id)
            .ValueGeneratedOnAdd();

        base.OnModelCreating(builder);
    }
}
