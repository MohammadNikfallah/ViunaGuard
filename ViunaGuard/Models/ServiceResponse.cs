namespace ViunaGuard.Models
{
    public class ServiceResponse <T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public int HttpResponseCode { get; set; } = 400;
    }
}
