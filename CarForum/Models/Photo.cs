namespace CarForum.Models;

public class Photo
{
    public int Id { get; set; }

    public string Path { get; set; }

    public int ReviewId { get; set; }

    public Review Review { get; set; }
}
