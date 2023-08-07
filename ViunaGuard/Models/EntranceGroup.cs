using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class EntranceGroup
    {
        [Key]
        public int Id { get; set; }
        [Required, ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [JsonIgnore]
        public Organization Organization { get; set; } = null!;
        [ForeignKey("Person")]
        public int? DriverId { get; set; }
        [JsonIgnore]
        public Person? Driver { get; set; }
        [ForeignKey("Car")]
        public int? CarId { get; set; }
        [JsonIgnore]
        public Car? Car { get; set; }
        //can entrance groups have multiple cars nad multiple drivers???
        public List<Entrance> Entrances { get; set; } = new List<Entrance>();
    }
}
