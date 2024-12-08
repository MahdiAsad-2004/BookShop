
namespace BookShop.Domain.Common.QueryOption
{
    public class Paging
    {
        public int ItemsInPage { get; private set; } = 12;
        public int PageNumber { get; private set; } = 1;

        private readonly int MaxItemsInPage = 48;

        public Paging()
        {

        }
        public Paging(int? pageNumber , int? itemsInPage)
        {
            if (itemsInPage != null && itemsInPage <= MaxItemsInPage)
                ItemsInPage = itemsInPage.Value;
            
            if(pageNumber != null && pageNumber > 0)
                PageNumber = pageNumber.Value;
        }
    }
}
