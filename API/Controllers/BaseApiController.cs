using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ServiceFilter(typeof(UserActivityLogger))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    { }
}
