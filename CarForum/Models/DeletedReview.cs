namespace CarForum.Models;

public class DeletedReview
{
    public int Id { get; set; }

    public string Title { get; set; }

    public int Rating { get; set; }

    public string CarBrand { get; set; }

    public string CarModel { get; set; }

    public int Year { get; set; }

    public int Mileage { get; set; }

    public string Transmission { get; set; }

    public string Body { get; set; }

    public string EngineType { get; set; }

    public string EngineCapacity { get; set; }

    public string DriveType { get; set; }

    public string Advantages { get; set; }

    public string Disadvantages { get; set; }

    public string OverallExperience { get; set; }

    public string AuthorId { get; set; }

    public User Author { get; set; }

    public string PhotoPath { get; set; }

    public DateTime PublishTime { get; set; }
}
