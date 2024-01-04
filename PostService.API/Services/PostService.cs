using PostService.Domain.Interfaces;
using PostService.Domain.Models;
using PostService.Infrastructure.Caching;

namespace PostService.API.Services;

public class PostService(
    IPostRepository postRepository,
    ICacheService<Post> cacheService) 
    : IPostService
{
    public async Task<Post?> AddPostAsync(Post post)
    {
        var res = await postRepository.AddPostAsync(post);

        if (res is not null)
        {
            // Set new to the redis instance
            cacheService.SetPost(post);
            return res;
        }

        return null;
    }

    public async Task<Post?> EditPostAsync(Post post)
    {
        var res = await postRepository.EditPostAsync(post);

        if (res is not null)
        {
            // Remove from Redis
            var getData = cacheService.GetData(post.Id);

            if (getData is not null
                && cacheService.RemovePost(getData.Id))
            {
                cacheService.SetPost(post);
            }

            return res;
        }

        return null;
    }

    public async Task<bool> DeletePostAsync(string id)
    {
        var getData = cacheService.GetData(id);
        if (getData is not null
            && cacheService.RemovePost(getData.Id))
        {
            var res = await postRepository.DeletePostAsync(id);
            return res is true;
        }

        return false;
    }

    public Post? GetPostById(string id)
    {
        var cache = cacheService.GetData(id);

        if (cache is not null)
        {
            return cache;
        }

        var res = postRepository.GetPostById(id);

        if (res is not null)
        {
            // var expirationTime = DateTimeOffset.Now.AddMinutes(60);
            cacheService.SetPost(res);

            return res;
        }

        return res;
    }

    public async Task<IList<Post>?> GetAllAsync()
    {
        var cache = await cacheService.GetAllAsync();

        if (cache != null)
        {
            return cache;
        }

        var res = postRepository.GetAll();

        if (res.Any())
        {
            foreach (var item in res)
            {
                cacheService.SetPost(item);
            }

            return res;
        }

        return res;
    }
}
