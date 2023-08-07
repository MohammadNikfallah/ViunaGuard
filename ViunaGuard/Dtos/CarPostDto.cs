using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViunaGuard.Models;

namespace ViunaGuard.Dtos
{
    public class CarPostDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string LicenseNumber { get; set; } = null!;
        public int? ColorId { get; set; }
        public int? BrandId { get; set; }
        public int? ModelId { get; set; }
        [StringLength(17)]
        public string? VIN { get; set; }
    }
}
