using BCTest.Models;
using Newtonsoft.Json;
using System.Text;

namespace BCTest.Services
{
    public class CarBrandServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string BaseUri = "https://api.businesscentral.dynamics.com/v2.0/873a7c07-4a74-4222-a9a0-22c8560049e0";
        private readonly TokenServices _tokenServices;
        private readonly TokenApplicationServices _tokenApplicationServices;

        public CarBrandServices(IHttpClientFactory httpClientFactory, TokenServices tokenServices, TokenApplicationServices tokenApplicationServices)
        {
            _httpClientFactory = httpClientFactory;
            _tokenServices = tokenServices;
            _tokenApplicationServices = tokenApplicationServices;
        }

        public async Task<List<CarBrand>> GetAllCarBrand()
        {
            //Setup BC access token
            await _tokenApplicationServices.GetBCConectionToken();

            List<CarBrand> carBrands = [];
            var url = "/Sandbox/api/phuong/demo/v2.0/carBrands?$top=2&company=CRONUS%20USA%2C%20Inc.";
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(BaseUri + url);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthenTokenModel.BusinessCentralAccessToken}");

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

        public async Task<bool> DeleteCarBrand(string carBrandId)
        {
            await _tokenApplicationServices.GetBCConectionToken();

            bool isDeleted = false;
            var url = $"/Sandbox/api/phuong/demo/v2.0/companies(3104717a-5377-ee11-817e-6045bdacaca5)/carBrands({carBrandId})";
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(BaseUri + url);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthenTokenModel.BusinessCentralAccessToken}");
            var response = await client.DeleteAsync(client.BaseAddress);

            if (response.IsSuccessStatusCode)
            {
                isDeleted = true;
            }
            return isDeleted;
        }


		public async Task<CarBrand> InsertCarBrand(CarBrand carBrand)
		{
			var url = "/Sandbox/api/phuong/demo/v2.0/companies(3104717a-5377-ee11-817e-6045bdacaca5)/carBrands";
			var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(BaseUri + url);
			client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthenTokenModel.AccessToken}");

			var json = JsonConvert.SerializeObject(carBrand);
			var data = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PostAsync(client.BaseAddress, data);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStringAsync();
				var insertedCarBrand = JsonConvert.DeserializeObject<CarBrand>(result);

				if (insertedCarBrand is null) return null;

				return insertedCarBrand;
			}

			return null;
		}


        public async Task<CarBrand> UpdateCarBrand(CarBrand carBrand)
        {
			var url = $"/Sandbox/api/phuong/demo/v2.0/companies(3104717a-5377-ee11-817e-6045bdacaca5)/carBrands({carBrand.Id})";
			var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(BaseUri + url);
			client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthenTokenModel.AccessToken}");
            client.DefaultRequestHeaders.Add("If-Match", "*");

			var json = JsonConvert.SerializeObject(carBrand);
			var data = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PutAsync(client.BaseAddress, data);
			if (response.IsSuccessStatusCode)
            {
				var result = await response.Content.ReadAsStringAsync();
				var updatedCarBrand = JsonConvert.DeserializeObject<CarBrand>(result);

				if (updatedCarBrand is null) return null;

				return updatedCarBrand;
			}

			return null;
		}


	}
}
