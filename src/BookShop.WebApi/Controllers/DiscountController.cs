using BookShop.Application.Features.Discount.Commands.Create;
using BookShop.Application.Features.Discount.Commands.Update;

namespace BookShop.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class DiscountController : BaseController
    {
        #region constructor
        public DiscountController(IMediator mediator) : base(mediator)
        {
        }

        #endregion


        [HttpPost()]
        public async Task<IActionResult> Create(CreateDiscountCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        

        [HttpPut()]
        public async Task<IActionResult> Update(UpdateDiscountCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        
     







    }
}
