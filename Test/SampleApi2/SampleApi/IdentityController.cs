using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SampleApi2.Controllers
{
    [Route("identity")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        public IdentityController()
        {
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Hello from sample api2");
        }
    }
}