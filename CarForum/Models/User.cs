using Microsoft.AspNetCore.Identity;

namespace CarForum.Models;

public class User : IdentityUser
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public override string? UserName { get; set; }

    public override string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public DateTime BirthDay { get; set; }

    public DateTime RegistrationDate { get; set; }

    public int DrivingExperienceYears { get; set; }
}
