using Microsoft.AspNetCore.Mvc;

namespace DataCollectorSpotify.Controllers
{
    [Route("HomeController")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return StatusCode(200);
        }
    }
}
