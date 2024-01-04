using PostService.Domain.Models;

namespace PostService.API.Services;

public interface IPostService
{
    Task<Post?> AddPostAsync(Post post);
    Task<Post?> EditPostAsync(Post post);
    Task<bool> DeletePostAsync(string id);
    Post? GetPostById(string id);
    Task<IList<Post>?> GetAllAsync();
}
