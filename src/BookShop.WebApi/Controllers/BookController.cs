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
            //_mediator.Send<>

            return Ok();
        }




    }
}
