using BCTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BCTest.Controllers
{
    public class ProductController : CustomControllerBase
    {
        private readonly ProductServices _productServices;

        public ProductController(ProductServices productServices)
        {
            _productServices = productServices;
        }

        [Authorize(Roles = "Manager,User")]
        [HttpGet("{barcode}")]
        public async Task<IActionResult> GetAProduct(string barcode)
        {
            var product = await _productServices.GetProductByBarcode(barcode);

            if (product is null) { return NotFound(); }

            return Ok(product);
        }

        [Authorize(Roles = "Manager,User")]
        [HttpGet]
        public async Task<IActionResult> GetPagedProduct(int? numOfRecords, int? currentPage, string? orderBy = null, string? orderString = null, string? filterBy = null, string? filterString = null)
        {
			// Set default values if not provided
			int defaultPageSize = 100;
			int defaultPage = 1;
			string defaultOrderBy = "ProductName"; // Set default orderBy to "name"
            string defaultOrderString = "asc";
            string defaultFilterBy = "ProductName";

			// If pageSize or page is not provided, use default values
			int actualPageSize = numOfRecords ?? defaultPageSize;
			int actualPage = currentPage ?? defaultPage;
			string actualOrderBy = string.IsNullOrWhiteSpace(orderBy) ? defaultOrderBy : orderBy;
            string actualOrderString = string.IsNullOrWhiteSpace(orderString) ? defaultOrderString : orderString;
            string actualFilterBy = string.IsNullOrWhiteSpace(filterBy) ? defaultFilterBy : filterBy;

			// Call the service method to get the paged car brands
			var response = await _productServices.GetPagedProduct(actualPageSize, actualPage, actualOrderBy, actualOrderString,actualFilterBy, filterString);

			// Handle the response from the service
			if (response.Item1.Count > 0)
			{
				return Ok(new { ProductList = response.Item1, TotalPage = response.Item2});
			}

			return BadRequest("Failed to retrieve data");
		}

        [Authorize(Roles = "Manager,User")]
        [HttpPatch("{productId}")]
        public async Task<IActionResult> PatchProduct(long productId, string barcode)
        {
            var isSuccess = await _productServices.SetProductBarcodeAsync(productId, barcode);
            if (isSuccess)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(long productId)
        {
            var isSuccess = await _productServices.DeleteProduct(productId);
            if (isSuccess)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
