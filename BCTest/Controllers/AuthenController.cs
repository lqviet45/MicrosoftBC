using BCTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace BCTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly TokenServices _tokenServices;

        public AuthenController(TokenServices tokenServices)
        {
            _tokenServices = tokenServices;
        }

        [HttpPost]
        public async Task<IActionResult> GetAccessToken(string redirectUri)
        {
            var url = await _tokenServices.LoginMicrosoft(redirectUri);

            return Ok(url);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> GetToken([FromQuery(Name = "code")] string code)
        {

            if (!code.IsNullOrEmpty())
            {
                string token = await _tokenServices.GetToken(code);
                return Ok(token);
            }
            return NotFound();
        }
    }
}
