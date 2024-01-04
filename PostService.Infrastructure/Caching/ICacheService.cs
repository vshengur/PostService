using PostService.Domain.Models;

namespace PostService.Infrastructure.Caching;

public interface ICacheService<T> where T : BaseObject, new()
{
    T? GetData(string id);
    Task<IList<T>?> GetAllAsync();
    T SetPost(T post);
    bool RemovePost(string id);
}
