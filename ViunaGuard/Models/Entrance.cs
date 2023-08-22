using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Models
{
    public class Entrance
    {
        [Key, Required]
        public int Id { get; set; }
        [Required, ForeignKey("Person")]
        public int PersonId { get; set; }
        [JsonIgnore]
        public Person? Person { get; set; }
        [ForeignKey("Car")]
        public int? CarId { get; set; }
        [JsonIgnore]
        public Car? Car { get; set; }
        public int? GuestCount { get; set; }
        [Required, ForeignKey("EntranceGroup")]
        public int EntranceGroupId { get; set; }
        [JsonIgnore]
        public EntranceGroup? EntranceGroup { get; set; }
        public bool IsDriver { get; set; } = false;
    }
}
