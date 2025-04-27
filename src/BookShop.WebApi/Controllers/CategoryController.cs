using BookShop.Application.Features.Category.Commands.Create;
using BookShop.Application.Features.Category.Commands.Update;

namespace BookShop.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class CategoryController : BaseController
    {
        #region constructor
        public CategoryController(IMediator mediator) : base(mediator)
        {
        }

        #endregion


        [HttpPost()]
        public async Task<IActionResult> Create(CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        

        [HttpPut()]
        public async Task<IActionResult> Update(UpdateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }








    }
}
