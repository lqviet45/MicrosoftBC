using BCTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BCTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarBrandController : ControllerBase
    {
        private readonly CarBrandServices _carBrandServices;
        private readonly TokenServices _tokenServices;

        public CarBrandController(CarBrandServices carBrandServices, TokenServices tokenServices)
        {
            _carBrandServices = carBrandServices;
            _tokenServices = tokenServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var isSuccess = HttpContext.Items.TryGetValue("Token", out object? newToken);

            var listResponse = await _carBrandServices.GetAllCarBrand();

            if (isSuccess && newToken != null)
            {
                var token = newToken.ToString();
                return Ok(new { listResponse, token });
            }
            return Ok(listResponse);
        }

        [HttpDelete("{carBrandId}")]
        public async Task<IActionResult> DeleteCarBrand(string carBrandId)
        {
            var isSuccess = await _carBrandServices.DeleteCarBrand(carBrandId);

            if (isSuccess) return NoContent();

            return NotFound(isSuccess);
        }
    }
}
