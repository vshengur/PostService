using PostService.Domain.Models;

namespace PostService.API.Services;

public interface IPostService
{
    Task<Post?> AddPostAsync(Post post);
    Task<Post?> EditPostAsync(Post post);
    Task<bool> DeletePostAsync(string id);
    Task<Post?> GetPostByIdAsync(string id);
    Task<IList<Post>?> GetAllAsync();
}
