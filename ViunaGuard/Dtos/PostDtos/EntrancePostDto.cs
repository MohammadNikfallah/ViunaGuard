using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViunaGuard.Dtos
{
    public class EntrancePostDto
    {
        [Required] 
        public string PersonId { get; set; } = null!;
        public int? CarId { get; set; }
        public int? GuestCount { get; set; }
        public bool IsDriver { get; set; } = false;
    }
}
