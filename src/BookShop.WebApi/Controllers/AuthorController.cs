using BookShop.Application.Features.Author.Commands.Create;
using BookShop.Application.Features.Author.Commands.Update;

namespace BookShop.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class AuthorController : BaseController
    {
        #region constructor
        public AuthorController(IMediator mediator) : base(mediator)
        {
        }

        #endregion


        [HttpPost()]
        public async Task<IActionResult> Create(CreateAuthorCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        

        [HttpPut()]
        public async Task<IActionResult> Update(UpdateAuthorCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }








    }
}
