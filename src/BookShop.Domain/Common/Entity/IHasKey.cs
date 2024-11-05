namespace BookShop.Domain.Common.Entity
{
    public interface IHasKey<TId>
    {
        public TId Id { get; set; }


    }
}
