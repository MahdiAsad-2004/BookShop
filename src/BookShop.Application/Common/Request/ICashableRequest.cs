using MediatR;

namespace BookShop.Application.Common.Request
{
    //public interface ICachableRequest : IRequest
    //{
    //    public string CacheKey { get; }
    //    public  TimeSpan CacheExpireTime { get; }
    //}

    //public interface ICachableRequest<TResponse> : IRequest<TResponse>
    //{
    //    public string CacheKey { get; init; }
    //    public TimeSpan CacheExpireTime { get; init; }
    //}


    public abstract class CachableRequest<TResponse> : IRequest<TResponse>
    {
        protected string _CacheKey { get; set; }
        public abstract TimeSpan CacheExpireTime { get; }
        public abstract string GetCacheKey();
    }

}
