using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System;
using ViunaGuard.Models.Enums;
using ViunaGuard.Models;

namespace ViunaGuard.Dtos
{
    public class CarGetDto
    {
        public int Id { get; set; }
        public string LicenseNumber { get; set; } = null!;
        [JsonIgnore]
        public int? ColorId { get; set; }
        public Color? Color { get; set; }
        [JsonIgnore]
        public int? BrandId { get; set; }
        public CarBrand? CarBrand { get; set; }
        [JsonIgnore]
        public int? ModelId { get; set; }
        public CarModel? CarModel { get; set; }
        public string? VIN { get; set; }
        public List<Person> People { get; set; } = new List<Person>();
    }
}
