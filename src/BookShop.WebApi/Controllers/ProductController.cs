using BookShop.Application.Features.Product.Dtos;
using BookShop.Application.Features.Product.Queries.GetSummaries;

namespace BookShop.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class ProductController : BaseController
    {
        #region constructor

        public ProductController(IMediator mediator) : base(mediator)
        {
        }

        #endregion


        [HttpGet]
        public async Task<IActionResult> GetAll(
            bool? Available = null, int? startPrice = null, int? endPrice = null, byte? score = null,
            int? itemCount = null, int? pageNumber = null , ProductSortingOrder? sort = null)
        {
            PaginatedDtos<ProductSummaryDto> paginatedProductSummaries = await _mediator.Send(new GetProductSummariesQuery
            {
                Available = Available,
                AverageScore = score,
                EndPrice = endPrice,
                SortingOrder = sort,
                StartPrice = startPrice,
                Paging = new Paging(itemCount , pageNumber),
                ProductType = null,
                Title = null
            });
            return Ok(paginatedProductSummaries);
        }






    }
}
