using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViunaGuard.Dtos
{
    public class EntrancePostDto
    {
        [Required]
        public int PersonId { get; set; }
        public int? CarId { get; set; }
        public int? GuestCount { get; set; }
        public bool IsDriver { get; set; } = false;
    }
}
