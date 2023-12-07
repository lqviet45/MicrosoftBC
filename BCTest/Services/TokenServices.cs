﻿using Azure.Core;
using Azure.Identity;
using BCTest.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json.Nodes;
using System.Web;

namespace BCTest.Services
{
    public class TokenServices
    {
        private readonly IConfiguration _configuration;

        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetToken(string code)
        {
           
            string clientId = _configuration["AzureAd:ClientId"] ?? throw new ArgumentNullException("The client Id is null!!");
            string tenantId = _configuration["AzureAd:TenantId"] ?? throw new ArgumentNullException("The tenant Id is null!!");
            string clientSecret = _configuration["AzureAd:ClientCredentials:ClientSecret"] ?? throw new ArgumentNullException("The client secret is null!!");

            string redirectUri = "https://localhost:7150/api/authen/callback";
            string URL = $"https://login.microsoftonline.com/{tenantId}";

            string[] scope = _configuration.GetSection("AzureAd:Scopes").Get<string[]>() ??
                throw new ArgumentNullException("The scopes is null");

            var pca = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithRedirectUri(redirectUri)
                .WithAuthority(new Uri(URL))
                .Build();
            var result = await pca.AcquireTokenByAuthorizationCode(scope, code)
                .ExecuteAsync();

            string token = result.AccessToken;
            AuthenTokenModel.AccessToken = token;
            AuthenTokenModel.RefeshToken = result.SpaAuthCode;

            return token;
        }

        public async Task<string> LoginMicrosoft()
        {
            string clientId = _configuration["AzureAd:ClientId"] ?? throw new ArgumentNullException("The client Id is null!!");
            string tenantId = _configuration["AzureAd:TenantId"] ?? throw new ArgumentNullException("The tenant Id is null!!");
            string clientSecret = _configuration["AzureAd:ClientCredentials:ClientSecret"] ?? throw new ArgumentNullException("The client secret is null!!");
            string[] scope = _configuration.GetSection("AzureAd:Scopes").Get<string[]>() ?? 
                throw new ArgumentNullException("The scopes is null");

            string redirectUri = "https://localhost:7150/api/authen/callback";

            string URL = $"https://login.microsoftonline.com/{tenantId}";

            var pca = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithRedirectUri(redirectUri)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(URL))
                .Build();

            var authProps = new AuthenticationProperties
            {
                RedirectUri = redirectUri,
            };

            Uri uri = await pca.GetAuthorizationRequestUrl(scope)
                .WithExtraQueryParameters(authProps.Parameters.ToString())
                .ExecuteAsync();

            return uri.ToString();
        }

    }
}
