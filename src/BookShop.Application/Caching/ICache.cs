
namespace BookShop.Application.Caching
{
    public interface ICache
    {
        void Add(string key, object item, TimeSpan expireTime);

        void Remove(string key);

        object? GetOrDefault(string key);
        
        TModel? GetOrDefault<TModel>(string key) where TModel : class;
    
    
    }
}
