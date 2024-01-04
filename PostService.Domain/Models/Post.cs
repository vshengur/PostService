namespace PostService.Domain.Models;

public class Post : BaseObject
{
    public string Title { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
}
