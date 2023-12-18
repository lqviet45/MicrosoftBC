using BCTest.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensibility;
using Microsoft.Identity.Web;
using static System.Net.WebRequestMethods;


namespace BCTest.Services
{
    public class TokenServices
    {
        private readonly IConfiguration _configuration;
        private readonly IConfidentialClientApplication _confidentialClientApplication;
        public static string? Token {  get; set; }

        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
            string clientId = _configuration["AzureAd:ClientId"] ?? throw new ArgumentNullException("The client Id is null!!");
            string tenantId = _configuration["AzureAd:TenantId"] ?? throw new ArgumentNullException("The tenant Id is null!!");
            string clientSecret = _configuration["AzureAd:ClientCredentials:ClientSecret"] ?? throw new ArgumentNullException("The client secret is null!!");

            string redirectUri = "https://localhost:7150/api/authen/callback";
            string URL = $"https://login.microsoftonline.com/{tenantId}";

            _confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithRedirectUri(redirectUri)
                .WithAuthority(new Uri(URL))
                .Build();
        }

        public async Task<string> GetToken(string code)
        {
            string[] scope = _configuration.GetSection("AzureAd:Scopes").Get<string[]>() ??
                throw new ArgumentNullException("The scopes is null");

            var result = await _confidentialClientApplication.AcquireTokenByAuthorizationCode(scope, code)
                .WithSpaAuthorizationCode()
                .ExecuteAsync();

            string token = result.AccessToken;
            AuthenTokenModel.BusinessCentralAccessToken = token;
            AuthenTokenModel.AuthCode = code;
            AuthenTokenModel.TokenExpriedDate = result.ExpiresOn;

            return token;
        }

        public async Task<string> CreateBCConectToken()
        {

            string[] scope = ["https://api.businesscentral.dynamics.com/API.ReadWrite.All",
                "https://api.businesscentral.dynamics.com/Automation.ReadWrite.All"];

            var result = await _confidentialClientApplication.AcquireTokenForClient(scope).ExecuteAsync();
            string token = result.AccessToken;

            return token;
        }

        public async Task<string> LoginMicrosoft(string redirecCode)
        {
            string[] scope = _configuration.GetSection("AzureAd:Scopes").Get<string[]>() ??
                throw new ArgumentNullException("The scopes is null");

            string redirectUri = $"{redirecCode}";

            var authProps = new AuthenticationProperties
            {
                RedirectUri = redirectUri,
            };

            Uri uri = await _confidentialClientApplication.GetAuthorizationRequestUrl(scope)
                .WithExtraQueryParameters(authProps.Parameters.ToString())
                .ExecuteAsync();

            return uri.ToString();
        }

        public async Task<string> GetNewAccessToken(string token)
        {
            string[] scope = _configuration.GetSection("AzureAd:Scopes").Get<string[]>() ??
                throw new ArgumentNullException("The scopes is null");

            var assertion = new UserAssertion(token);

            try
            {
                var result = await _confidentialClientApplication.AcquireTokenOnBehalfOf(scope, assertion)
                    .ExecuteAsync();


                token = result.AccessToken;

                return token;
            }
            catch (MsalUiRequiredException)
            {
                return token;
            }
        }
    }
}
