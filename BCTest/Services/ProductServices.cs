using BCTest.Helper;
using BCTest.Models;
using Newtonsoft.Json;
using System;
using System.Text;

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
            string url = $"/Sandbox/api/phuong/demo/v1.0/companies(3104717a-5377-ee11-817e-6045bdacaca5)/Products?$filter=barcode eq '{barcode}'";
            var uri = new Uri(BaseUri + url);

            var client = _httpClientFactory.CreateClient();

            var response = await BusinessCentralClientCall<Product>.CallApi(client, uri, Method.GET);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var resopnse = JsonConvert.DeserializeObject<Response<List<Product>>>(data);
                return resopnse?.Value?.First();
            }
            return null;
        }

		public async Task<(List<Product>, int)> GetPagedProduct(int numOfRecords, int currentPage, string? orderBy = null, string? orderString = null, string? filterBy = null, string? filterString = null)
		{
			await _tokenApplicationServices.GetBCConectionToken();

			var listResponse = new List<Product>();

			// Build the URL based on parameters
			var urlBuilder = new StringBuilder($"/Sandbox/api/phuong/demo/v1.0/companies(3104717a-5377-ee11-817e-6045bdacaca5)/Products?$count=true&$top={numOfRecords}&$skip={(currentPage - 1) * numOfRecords}&company=CRONUS%20USA%2C%20Inc.");

			// Add OrderBy to the URL if provided
			urlBuilder.Append($"&$orderby={orderBy}");

			if(!string.IsNullOrWhiteSpace(orderString) || (orderString == "asc" || orderString == "desc"))
			{
				urlBuilder.Append($" {orderString}");
			}

			// Add Filter to the URL if provided
			if (!string.IsNullOrWhiteSpace(filterBy) && !string.IsNullOrEmpty(filterString))
			{
				// Note: You may need to handle special characters and encoding based on your requirements
				//urlBuilder.Append($"&$filter={filterBy} eq '{filterString}'");
				urlBuilder.Append($"&$filter=contains({filterBy}, '{filterString}')");
			}


			var uri = new Uri(BaseUri + urlBuilder);

			var client = _httpClientFactory.CreateClient();

			var response = await BusinessCentralClientCall<Product>.CallApi(client, uri, Method.GET);

			int TotalRecords = 0;
			int TotalPages = 0;
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var responseObject = JsonConvert.DeserializeObject<Response<List<Product>>>(data);

				if (responseObject != null && responseObject.Value != null)
				{
					listResponse = responseObject.Value;
				}

				// Extract total records from the response headers
				if (responseObject != null && responseObject.OdataCount >= 0)
				{
					TotalRecords = responseObject.OdataCount;
				}

				// Calculate TotalPages
				 TotalPages = (int)Math.Ceiling((double)TotalRecords / numOfRecords);
			}

			return (listResponse, TotalPages);
		}

		public async Task<bool> SetProductBarcodeAsync(long productId, string barcode)
		{
			await _tokenApplicationServices.GetBCConectionToken();
			string url = $"/Sandbox/api/phuong/demo/v1.0/companies(3104717a-5377-ee11-817e-6045bdacaca5)/Products({productId})";
            var uri = new Uri(BaseUri + url);
			var client = _httpClientFactory.CreateClient();
			var product = new
			{
				Barcode = barcode
			};
			
			var response = await BusinessCentralClientCall<Product>.CallApi(client, uri, Method.PUT, product);

			if (response.IsSuccessStatusCode)
			{
				return true;
			}

			return false;
		}

		public async Task<bool> DeleteProduct(long productId)
		{
            await _tokenApplicationServices.GetBCConectionToken();
            string url = $"/Sandbox/api/phuong/demo/v1.0/companies(3104717a-5377-ee11-817e-6045bdacaca5)/Products({productId})";
            var uri = new Uri(BaseUri + url);
            var client = _httpClientFactory.CreateClient();
			var response = await BusinessCentralClientCall<Product>.CallApi(client, uri, Method.DELETE);

			if (response.IsSuccessStatusCode)
			{
				return true;
			}

			return false;
        }
    }
}
