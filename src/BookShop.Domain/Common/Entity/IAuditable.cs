namespace BookShop.Domain.Common.Entity
{
    public interface IAuditable
    {
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string CreateBy { get; set; }
        public string LastModifiedBy { get; set; }


    }
}
