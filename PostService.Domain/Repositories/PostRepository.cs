using PostService.Domain.DbContexts;
using PostService.Domain.Interfaces;
using PostService.Domain.Models;

namespace PostService.API.Services;

public class PostRepository(AppDbContext dbContext) : IPostRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Post?> AddPostAsync(Post post)
    {
        if (post is not null)
        {
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();
        }
        return post;
    }

    public async Task<bool> DeletePostAsync(string id)
    {
        var post = _dbContext.Posts.FirstOrDefault(p => p.Id == id);
        if (post is not null)
        {
            _dbContext.Posts.Remove(post);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Post?> EditPostAsync(Post post)
    {
        var postRes = _dbContext.Posts.FirstOrDefault(p => p.Id == post.Id);
        if (postRes is not null)
        {
            postRes.Title = post.Title;
            postRes.UserName = post.UserName;

            await _dbContext.SaveChangesAsync();
            return post;
        }
        return null;
    }

    public IList<Post> GetAll()
    {
        return _dbContext.Posts.ToList();
    }

    public Post? GetPostById(string id) 
        => _dbContext.Posts.FirstOrDefault(p => p.Id == id);
}
