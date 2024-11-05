using BookShop.Application.Caching;
using BookShop.Application.Common.Request;
using MediatR;
using Serilog;

namespace BookShop.Application.Behaviours
{
    public class CacheBahviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICashableRequest
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
            _logger.Information($"Reading {nameof(request)} request with CacheKey ({request.CashKey}) from chach.");
            TResponse? response = _cache.GetOrDefault<TResponse>(request.CashKey);

            if (response != null)
            {
                _logger.Information($"{nameof(request)} request with CacheKey {request.CashKey} exist in cache.");
                return Task.FromResult(response);
            }
            _logger.Information($"{nameof(request)} request with CacheKey {request.CashKey} not exist in cache.");

            response = next()
                .GetAwaiter()
                .GetResult();
            _cache.Add(request.CashKey, response, request.ExpireTime);
            _logger.Information($"{nameof(request)} response added to cache");

            return next();
        }
    }
}
