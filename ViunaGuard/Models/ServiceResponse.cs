using Microsoft.EntityFrameworkCore;

namespace ViunaGuard.Models
{
    [Keyless]
    public class ServiceResponse <T> : ServiceResponse
    {
        public T? Data { get; set; }
    }
    public class ServiceResponse
    {
        public string? Message { get; set; }
        public int HttpResponseCode { get; set; } = 400;
    }
}
