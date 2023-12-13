using Newtonsoft.Json;

namespace BCTest.Models
{
    public class Response<T> where T : class
    {
        public string? Context { get; set; }
        public T? Value { get; set; }

        [JsonProperty("@odata.count")]
        public int OdataCount { get; set; }
    }
}
