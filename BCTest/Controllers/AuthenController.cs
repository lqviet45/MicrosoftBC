using BCTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web.Resource;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text.Json;

namespace BCTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly Uri path = new Uri("https://api.businesscentral.dynamics.com/v2.0/873a7c07-4a74-4222-a9a0-22c8560049e0/Sandbox/api/phuong/demo/v2.0/carBrands?company=CRONUS%20USA%2C%20Inc.");
        private readonly TokenServices _tokenServices;

        public AuthenController(TokenServices tokenServices)
        {
            _tokenServices = tokenServices;
        }

        [HttpPost]
        public async Task<IActionResult> GetAccessToken()
        {
            var token = await _tokenServices.LoginMicrosoft();

            return Ok(token);
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

        [Authorize(Roles = "Manager")]
        [HttpGet("test")]
        public IActionResult SetProduct()
        {
            return Ok();
        }
    }
}
