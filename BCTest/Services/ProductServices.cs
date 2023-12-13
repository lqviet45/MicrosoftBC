namespace BCTest.Services
{
    public class ProductServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string BaseUri = "https://api.businesscentral.dynamics.com/v2.0/873a7c07-4a74-4222-a9a0-22c8560049e0";
        private readonly TokenServices _tokenServices;
        private readonly TokenApplicationServices _tokenApplicationServices;

        public ProductServices(IHttpClientFactory httpClientFactory, TokenServices tokenServices, TokenApplicationServices tokenApplicationServices)
        {
            _httpClientFactory = httpClientFactory;
            _tokenServices = tokenServices;
            _tokenApplicationServices = tokenApplicationServices;
        }


    }
}
