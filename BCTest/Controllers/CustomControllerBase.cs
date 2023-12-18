using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BCTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {
    }
}
