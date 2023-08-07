using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Text.Json.Serialization;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string LicenseNumber { get; set; } = null!;
        [ForeignKey("Color")]
        public int? ColorId { get; set; }
        public Enums.Color? Color { get; set; }
        [ForeignKey("CarBrand")]
        public int? BrandId { get; set; }
        public CarBrand? CarBrand { get; set; }
        [ForeignKey("CarModel")]
        public int? ModelId { get; set; }
        public CarModel? CarModel { get; set; }
        [StringLength(17)]
        public string? VIN { get; set; }
        [JsonIgnore]
        public List<Person> People { get; set; } = new List<Person>();
    }
}
