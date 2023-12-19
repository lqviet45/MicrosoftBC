using BCTest.Models;
using Newtonsoft.Json;
using System.Text;

namespace BCTest.Helper
{
    public static class BusinessCentralClientCall<T> where T : class
    {
        public static async Task<HttpResponseMessage> CallApi(HttpClient httpClient, Uri url, Method method, object? obj = null)
        {
            if ((method == Method.PUT || method == Method.POST) && obj is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            var resopnse = new HttpResponseMessage();
            string json;
            StringContent data;
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthenTokenModel.BusinessCentralAccessToken}");
            switch (method)
            {
                case Method.GET:
                    resopnse = await httpClient.GetAsync(url);
                    break;
                case Method.POST:
                    json = JsonConvert.SerializeObject(obj);
                    data = new StringContent(json, Encoding.UTF8, "application/json");
                    resopnse = await httpClient.PostAsync(url, data);
                    break;
                case Method.PUT:
                    httpClient.DefaultRequestHeaders.Add("If-Match", "*");
                    json = JsonConvert.SerializeObject(obj);
                    data = new StringContent(json, Encoding.UTF8, "application/json");
                    resopnse = await httpClient.PutAsync(url, data);
                    break;
                case Method.DELETE:
                    resopnse = await httpClient.DeleteAsync(url);
                    break;
            }
            return resopnse;
        }
    }

    public enum Method
    {
        GET, POST, PUT, DELETE
    }
}
