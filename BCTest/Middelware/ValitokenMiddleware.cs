using BCTest.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace BCTest.Middelware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ValitokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenServices _tokenServices;

        public ValitokenMiddleware(RequestDelegate next, TokenServices tokenServices)
        {
            _next = next;
            _tokenServices = tokenServices;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var isExistToken = httpContext.Request.Headers.TryGetValue("Authorization", out StringValues tokenHeader);
            if (isExistToken)
            {
                var token = tokenHeader.ToString().Split(" ");

                var jwtToken = new JwtSecurityToken(token[1]);
                if (jwtToken.ValidTo <= DateTime.UtcNow.AddMinutes(5))
                {
                    var newToken = await _tokenServices.GetNewAccessToken(token[1]);
                    httpContext.Items.Add("Token", $"{newToken}");
                }
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ValitokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseValitokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValitokenMiddleware>();
        }
    }
}
