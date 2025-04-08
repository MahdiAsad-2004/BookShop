using BookShop.Application.Common.Dtos;
using BookShop.Application.Features.Book.Dtos;
using BookShop.Application.Features.Book.Queries.GetSummaries;
using BookShop.Domain.Common.QueryOption;
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


        [ActionName("/")]
        public async Task<IActionResult> GetAll(
            bool? Available = null, int? startPrice = null, int? endPrice = null, byte? score = null,
            int? itemCount = null, int? pageNumber = null , BookSortingOrder? sort = null)
        {
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await _mediator.Send(new GetBookSummariesQuery
            {
                IsAvailable = Available,
                AverageScore = score,
                EndPrice = endPrice,
                SortingOrder = sort,
                StartPrice = startPrice,
                Paging = new Paging(itemCount , pageNumber),
            });
            return Ok(paginatedBookSummaries);
        }




    }
}
