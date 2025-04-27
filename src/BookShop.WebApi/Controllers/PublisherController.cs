using BookShop.Application.Features.Publisher.Commands.Create;
using BookShop.Application.Features.Publisher.Commands.Update;

namespace BookShop.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class PublisherController : BaseController
    {
        #region constructor
        public PublisherController(IMediator mediator) : base(mediator)
        {
        }

        #endregion


        [HttpPost()]
        public async Task<IActionResult> Create(CreatePublisherCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        

        [HttpPut()]
        public async Task<IActionResult> Update(UpdatePublisherCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }








    }
}
