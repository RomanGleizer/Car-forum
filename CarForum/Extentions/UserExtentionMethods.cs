using CarForum.Models;
using CarForum.ViewModels;

namespace CarForum.Extentions;

public static class UserExtentionMethods
{
    public static User Convert(this User user, EditViewModel model)
    {
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.PhoneNumber = model.Phone;
        user.Country = model.Country;
        user.City = model.City;
        user.BirthDay = model.BirthDay;
        user.DrivingExperienceYears = model.DrivingExperienceYears;

        return user;
    }
}
