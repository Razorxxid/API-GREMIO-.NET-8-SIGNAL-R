using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PWA_GREMIO_API.Core.Entities;

namespace PWA_GREMIO_API.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementController : ControllerBase
    {
        [HttpGet] // Specify HTTP method explicitly
        public IActionResult Get()
        {
            // Your action logic here
            return Ok();
        }

        // Other actions in the controller
    }
}
