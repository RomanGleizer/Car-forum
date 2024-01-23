using CarForum.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CarForum.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public override string Email { get; set; }

    public override string PhoneNumber { get; set; }

    public string Country { get; set; }

    public string City { get; set; }

    public DateTime BirthDay { get; set; }

    public DateTime RegistrationDate { get; set; }

    public int DrivingExperienceYears { get; set; }

    public List<Review> Reviews { get; set; } = new List<Review>();

    public List<DeletedReviewViewModel> DeletedReviews { get; set; } = new List<DeletedReviewViewModel>();
}
