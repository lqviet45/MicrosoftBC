using BCTest.Services;
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

        [HttpGet("{barcode}")]
        public async Task<IActionResult> GetAProduct(string barcode)
        {
            var product = await _productServices.GetProductByBarcode(barcode);

            if (product is null) { return NotFound(); }

            return Ok(product);
        }
    }
}
