
using MediatR;

namespace BookShop.Application.Common.Request
{
    public interface ICashableRequest : IRequest
    {
        public string CashKey { get; init; }
        public  TimeSpan ExpireTime { get; init; }
    }

    public interface ICashableRequest<TResponse> : IRequest<TResponse>
    {
        public string CashKey { get; init; }
        public  DateTimeOffset ExpireTime { get; init; }

    }

}
