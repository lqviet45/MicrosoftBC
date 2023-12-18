using BCTest.Models;
using BCTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BCTest.Controllers
{
    public class CarBrandController : CustomControllerBase
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
            var listResponse = await _carBrandServices.GetAllCarBrand();
            return Ok(listResponse);
        }

        [HttpGet("{carBrandId}")]
        public async Task<IActionResult> GetByBarcodeId(string carBrandId)
        {
            var carBrand = await _carBrandServices.GetCarBrandByBarcodeId(carBrandId);
            if (carBrand is null)
            {
                return NotFound($"The car brand with this {carBrandId} doesn't exist!");
            }
            return Ok(carBrand);
        }

        [HttpDelete("{carBrandId}")]
        public async Task<IActionResult> DeleteCarBrand(string carBrandId)
        {
            var isSuccess = await _carBrandServices.DeleteCarBrand(carBrandId);

            if (isSuccess) return NoContent();

            return NotFound(isSuccess);
        }


		[HttpPost]
		public async Task<IActionResult> InsertCarBrand(CarBrand carBrand)
		{
			var response = await _carBrandServices.InsertCarBrand(carBrand);
			return Ok(response);
		}

        [HttpPut]
        public async Task<IActionResult> UpdateCarBrand(CarBrand carBrand)
        {
			var response = await _carBrandServices.UpdateCarBrand(carBrand);
			return Ok(response);
		}

		[HttpGet("page")]
		public async Task<IActionResult> GetCarBrandByPage(int? pageSize, int? page, string? orderBy = null, string? filterby = null, string? filterString = null)
		{
			// Set default values if not provided
			int defaultPageSize = 100;
			int defaultPage = 1;
			string defaultOrderBy = "name"; // Set default orderBy to "name"

			// If pageSize or page is not provided, use default values
			int actualPageSize = pageSize ?? defaultPageSize;
			int actualPage = page ?? defaultPage;
			string actualOrderBy = string.IsNullOrWhiteSpace(orderBy) ? defaultOrderBy : orderBy;

			// Call the service method to get the paged car brands
			var response = await _carBrandServices.GetPagedCarBrands(actualPageSize, actualPage, actualOrderBy, filterby, filterString);

			// Handle the response from the service
			if (response != null)
			{
				return Ok(response);
			}

			return BadRequest("Failed to retrieve data");
		}

	}
}
