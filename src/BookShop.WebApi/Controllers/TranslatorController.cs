using BookShop.Application.Features.Translator.Commands.Create;
using BookShop.Application.Features.Translator.Commands.Update;

namespace BookShop.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class TranslatorController : BaseController
    {
        #region constructor
        public TranslatorController(IMediator mediator) : base(mediator)
        {
        }

        #endregion


        [HttpPost()]
        public async Task<IActionResult> Create(CreateTranslatorCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
            
        [HttpPut()]
        public async Task<IActionResult> Update(UpdateTranslatorCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }








    }
}
