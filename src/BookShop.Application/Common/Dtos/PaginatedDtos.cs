
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;

namespace BookShop.Application.Common.Dtos
{
    public class PaginatedDtos<TDto> where TDto : BaseDto
    {
        public List<TDto> Dtos { get; private set; }
        public int PageNumber { get; private set; }
        public int ItemsInPage { get; private set; }
        public int LastPageNumber { get; private set; }
        public int TotalItemCount { get; private set; }
        public bool IsPaginated { get; private set; }



        public PaginatedDtos(List<TDto> dtos, Paging? paging, int totalItemCount)
        {
            Dtos = dtos;
            IsPaginated = paging != null;
            TotalItemCount = totalItemCount;
            PageNumber = paging != null ? paging.PageNumber : 1;
            ItemsInPage = paging != null ? paging.ItemsInPage : TotalItemCount;
            LastPageNumber = (int)Math.Ceiling((decimal)TotalItemCount / ItemsInPage);
        }

        
    }
}
