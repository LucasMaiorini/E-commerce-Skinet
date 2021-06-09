using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //The ApiController's behavior is modified in Startup.cs <ApiBehaviorOptions>
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
    }
}