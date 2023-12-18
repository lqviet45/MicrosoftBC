using BCTest.Helper;
using BCTest.Models;
using Newtonsoft.Json;

namespace BCTest.Services
{
    public class ProductServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string BaseUri = "https://api.businesscentral.dynamics.com/v2.0/873a7c07-4a74-4222-a9a0-22c8560049e0";
        private readonly TokenApplicationServices _tokenApplicationServices;

        public ProductServices(IHttpClientFactory httpClientFactory, TokenServices tokenServices, TokenApplicationServices tokenApplicationServices)
        {
            _httpClientFactory = httpClientFactory;
            _tokenApplicationServices = tokenApplicationServices;
        }

        public async Task<Product?> GetProductByBarcode(string barcode)
        {
            await _tokenApplicationServices.GetBCConectionToken();
            string url = $"/Sandbox/api/phuong/demo/v1.0/companies(3104717a-5377-ee11-817e-6045bdacaca5)/Products('{barcode}')";
            var uri = new Uri(BaseUri + url);

            var client = _httpClientFactory.CreateClient();

            var response = await BusinessCentralClientCall<Product>.CallApi(client, uri, Method.GET);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(data);
                return product;
            }
            return null;
        }
    }
}
