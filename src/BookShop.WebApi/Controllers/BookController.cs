using BookShop.Application.Features.Book.Commands.Create;
using BookShop.Application.Features.Book.Commands.Update;
using BookShop.Application.Features.Book.Queries.GetDetail;
using BookShop.Application.Features.Product.Dtos;
using BookShop.Application.Features.Product.Queries.GetSummaries;

namespace BookShop.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class BookController : BaseController
    {
        #region constructor
       
        public BookController(IMediator mediator) : base(mediator)
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
                ProductType = ProductType.Book,
            });
            return Ok(paginatedProductSummaries);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateBookCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        

        [HttpPut]
        public async Task<IActionResult> Update(UpdateBookCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new GetBookDetailQuery
            {
                Id = id,
            });
            return Ok(result);
        }













    }
}
