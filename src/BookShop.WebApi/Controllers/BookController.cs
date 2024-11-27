using BookShop.Application.Features.Product.Dtos;
using BookShop.Application.Features.Product.Queries.GetSummaries;
using BookShop.Application.Features.Product.Queries.GetSummary;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Enums;
using BookShop.Domain.QueryOptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        #region constructor

        private readonly IMediator _mediator;
        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }


        #endregion


        [ActionName("get/{id}")]
        public async Task<IActionResult> Get(Guid guid)
        {
            ProductSummaryDto productSummaryDto = await _mediator.Send(new GetProductSummaryQuery
            {
                Id = guid,
                ProductType = ProductType.Book,
            });
            return Ok(productSummaryDto);
        }


        [ActionName("/")]
        public async Task<IActionResult> GetAll(
            bool? Available = null, int? startPrice = null, int? endPrice = null, byte? score = null,
            int? itemCount = null, int? pageNumber = null , ProductSortingOrder? sort = null)
        {
            List<ProductSummaryDto> productSummaryDtos = await _mediator.Send(new GetProductSummariesQuery
            {
                Available = Available,
                AverageScore = score,
                EndPrice = endPrice,
                ProductType = ProductType.Book,
                SortingOrder = sort,
                StartPrice = startPrice,
                Paging = new Paging(itemCount , pageNumber),
            });
            return Ok(productSummaryDtos);
        }




    }
}
