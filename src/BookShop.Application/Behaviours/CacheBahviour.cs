using BookShop.Application.Caching;
using BookShop.Application.Common.Request;
using MediatR;
using Serilog;

namespace BookShop.Application.Behaviours
{
    public class CacheBahviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : CachableRequest<TResponse>
        where TResponse : class
    {

        #region constructor

        private readonly ICache _cache;
        private readonly ILogger _logger;
        public CacheBahviour(ICache cache, ILogger logger)
        {
            _cache = cache;
            _logger = logger;
        }

        #endregion


        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.Information($"Reading {typeof(TRequest).Name} request with CacheKey ({request.GetCacheKey()}) from chach.");
            string cacheKey = request.GetCacheKey();
            TResponse? response = _cache.GetOrDefault<TResponse>(cacheKey);

            if (response != null)
            {
                _logger.Information($"{typeof(TRequest).Name} request with CacheKey {cacheKey} exist in cache.");
                return Task.FromResult(response);
            }
            _logger.Information($"{typeof(TRequest).Name} request with CacheKey {cacheKey} not exist in cache.");

            response = next()
                .GetAwaiter()
                .GetResult();
            _cache.Add(request.GetCacheKey(), response, request.CacheExpireTime);
            _logger.Information($"{typeof(TRequest).Name} response added to cache");


            return Task.FromResult(response);
            //return next();
        }
    }
}
