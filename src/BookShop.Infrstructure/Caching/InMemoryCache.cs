using BookShop.Application.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace BookShop.Infrastructure.Caching
{
    internal class InMemoryCache : ICache
    {

        #region constructor

        private readonly IMemoryCache _memoryCache;
        public InMemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        #endregion


        public void Add(string key, object item, TimeSpan expireTime)
        {
            _memoryCache.Set(key, item);
        }

        public object? GetOrDefault(string key)
        {
            return _memoryCache.Get(key);
        }

        public TModel? GetOrDefault<TModel>(string key) where TModel : class
        {
            var a = _memoryCache.Get(key);
            return _memoryCache.Get<TModel>(key);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

    }
}
