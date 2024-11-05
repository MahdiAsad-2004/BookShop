namespace BookShop.Domain.Common.Entity
{
    public interface ISoftDalete
    {
        public DateTime? DeleteDate { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
