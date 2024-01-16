namespace CarForum.Models;

public class Comment
{
    public int Id { get; set; }

    public User Author { get; set; }
    
    public string Text { get; set; }

    public DateTime PublishTime { get; set; }
}
