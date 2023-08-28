using Microsoft.EntityFrameworkCore;

namespace ViunaGuard.Models
{
    [Keyless]
    public class ServiceResponse <T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public int HttpResponseCode { get; set; } = 400;
    }
    public class ServiceResponse
    {
        public string? Message { get; set; }
        public int HttpResponseCode { get; set; } = 400;
    }
}
