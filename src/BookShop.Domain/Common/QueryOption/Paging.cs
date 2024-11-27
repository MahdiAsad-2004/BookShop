
namespace BookShop.Domain.Common.QueryOption
{
    public class Paging
    {
        public int ItemCount { get; private set; } = 12;
        public int PageNumber { get; private set; } = 1;

        private readonly int MaxItemCount = 48;

        public Paging()
        {

        }
        public Paging(int? itemCount, int? pageNumber)
        {
            if (itemCount != null && itemCount <= MaxItemCount)
                ItemCount = itemCount.Value;
            
            if(pageNumber != null && pageNumber > 0)
                PageNumber = pageNumber.Value;
        }
    }
}
