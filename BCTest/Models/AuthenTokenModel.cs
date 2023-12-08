using Microsoft.Identity.Client;

namespace BCTest.Models
{
    public static class AuthenTokenModel
    {
        public static string? BusinessCentralAccessToken { get; set; }
        public static DateTimeOffset TokenExpriedDate { get; set; }
        public static string? AuthCode { get; set; }
    }
}
