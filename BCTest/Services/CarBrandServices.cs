using BCTest.Models;
using Newtonsoft.Json;

namespace BCTest.Services
{
    public class CarBrandServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string BaseUri = "https://api.businesscentral.dynamics.com/v2.0/873a7c07-4a74-4222-a9a0-22c8560049e0";

        public CarBrandServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<CarBrand>> GetAllCarBrand()
        {
            List<CarBrand> carBrands = new List<CarBrand>();
            var url = "/Sandbox/api/phuong/demo/v2.0/carBrands?$top=2000&company=CRONUS%20USA%2C%20Inc.";
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(BaseUri + url);
            //client.DefaultRequestHeaders.Authorization = new($"bearer {AuthenTokenModel.AccessToken}");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthenTokenModel.AccessToken}");

            var response = await client.GetAsync(client.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var responeObject = JsonConvert.DeserializeObject<Response<List<CarBrand>>>(data);

                if (responeObject is null || responeObject.Value is null) return carBrands;

                carBrands = responeObject.Value;
            }

            return carBrands;
        }
    }
}
