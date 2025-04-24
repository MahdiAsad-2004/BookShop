
namespace BookShop.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class HomeController : BaseController
{
    #region constructor
    public HomeController(IMediator mediator) : base(mediator)
    {
    }

    #endregion


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return Content("Hi....");
    }




}
