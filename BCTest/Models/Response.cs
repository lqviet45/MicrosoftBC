namespace BCTest.Models
{
    public class Response<T> where T : class
    {
        public string? Context { get; set; }
        public T? Value { get; set; }
    }
}
