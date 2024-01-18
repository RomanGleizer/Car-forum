using CarForum.Models;
using CarForum.ViewModels;

namespace CarForum.Extentions;

public static class ReviewExtentionMethods
{
    public static Review Convert(this Review review, EditReviewViewModel model)
    {
        review.Title = model.Title;
        review.Rating = model.Rating;
        review.CarBrand = model.CarBrand;
        review.CarModel = model.CarModel;
        review.Transmission = model.Transmission;
        review.Body = model.Body;
        review.Year = model.Year;
        review.Mileage = model.Mileage;
        review.Advantages = model.Advantages;
        review.Disadvantages = model.Disadvantages;
        review.OverallExperience = model.OverallExperience;
        review.DriveType = model.DriveType;
        review.EngineCapacity = model.EngineCapacity;
        review.EngineType = model.EngineType;

        return review;
    }
}
