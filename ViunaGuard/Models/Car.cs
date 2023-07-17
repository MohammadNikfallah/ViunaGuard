using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class Car
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Owner")]
        public Guid? OwnerId { get; set; }
        [JsonIgnore]
        public Person? Owner { get; set; } //Table joda
        public int? ColorId { get; set; }
        public int? BrandId { get; set; }
        public int? ModelId { get; set; }
        public string? LicenseNumber { get; set; }

    }
}
