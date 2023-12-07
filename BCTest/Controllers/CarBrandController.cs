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

        public CarBrandController(CarBrandServices carBrandServices)
        {
            _carBrandServices = carBrandServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var listResponse = await _carBrandServices.GetAllCarBrand();
            return Ok(listResponse);
        }
    }
}
