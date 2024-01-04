using PostService.Domain.Models;

namespace PostService.Domain.Interfaces;

public interface IPostRepository
{
    Task<Post?> AddPostAsync(Post post);
    Task<Post?> EditPostAsync(Post post);
    Task<bool> DeletePostAsync(string id);
    Post? GetPostById(string id);
    IList<Post> GetAll();
}
