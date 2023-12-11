using Azure.Core;
using Azure.Identity;
using BCTest.Models;
using Microsoft.Identity.Client;

namespace BCTest.Services
{
    public class TokenApplicationServices
    {
        private readonly IConfiguration _configuration;

        public TokenApplicationServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task GetBCConectionToken()
        {
            if (AuthenTokenModel.TokenExpriedDate <= DateTimeOffset.UtcNow || AuthenTokenModel.BusinessCentralAccessToken is null)
            {
                string clientId = _configuration["AzureAd:ClientId"] ?? throw new ArgumentNullException("The client Id is null!!");
                string tenantId = _configuration["AzureAd:TenantId"] ?? throw new ArgumentNullException("The tenant Id is null!!");
                string clientSecret = _configuration["AzureAd:ClientCredentials:ClientSecret"] ?? throw new ArgumentNullException("The client secret is null!!");
                string[] scope = ["https://api.businesscentral.dynamics.com/.default"];

                var tokenRequest = new TokenRequestContext(scope);
                ClientSecretCredential client = new ClientSecretCredential(tenantId, clientId, clientSecret);

                var token = await client.GetTokenAsync(tokenRequest);
                AuthenTokenModel.BusinessCentralAccessToken = token.Token;
                AuthenTokenModel.TokenExpriedDate = token.ExpiresOn;
            }
        }
    }
}
