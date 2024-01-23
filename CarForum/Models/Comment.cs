namespace CarForum.Models;

public class Comment
{
    public int Id { get; set; }

    public User Author { get; set; }
    
    public int ReviewId { get; set; }
    
    public Review Review { get; set; }

    public string Text { get; set; }

    public int LikesAmount { get; set; }

    public DateTime PublishTime { get; set; }

    public List<User> LikedByUsers { get; set; } = new List<User>();
}
