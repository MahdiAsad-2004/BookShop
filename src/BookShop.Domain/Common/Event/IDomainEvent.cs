
namespace BookShop.Domain.Common.Event
{
    public interface IDomainEvent
    {
        public DateTime EventDateTime { get; init; }
        public Guid UserId { get; init; }


    }


}
