using BookShop.Application.Features.EBook.Commands.Create;
using BookShop.Application.Features.EBook.Commands.Update;
using BookShop.Application.Features.Product.Dtos;
using BookShop.Application.Features.Product.Queries.GetSummaries;

namespace BookShop.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class EBookController : BaseController
    {
        #region constructor
        public EBookController(IMediator mediator) : base(mediator)
        {
        }

        #endregion


        [HttpGet("/")]
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
                ProductType = ProductType.EBook,
            });
            return Ok(paginatedProductSummaries);
        }


        [HttpPost("/")]
        public async Task<IActionResult> Create(CreateEBookCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        

        [HttpPut("/")]
        public async Task<IActionResult> Update(UpdateEBookCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(Guid id)
        //{
        //    var result = await _mediator.Send(new GetEBookDetailQuery
        //    {
        //        Id = id,
        //    });
        //    return Ok(result);
        //}













    }
}
