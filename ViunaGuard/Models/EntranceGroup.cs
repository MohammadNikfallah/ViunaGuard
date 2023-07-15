using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class EntranceGroup
    {
        [Key]
        public Guid Id { get; set; }
        [Required, ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [JsonIgnore]
        public Organization Organization { get; set; }
        [Required, ForeignKey("Person")]
        public Guid PersonId { get; set; }
        [JsonIgnore]
        public Person? Person { get; set; }
        [ForeignKey("Car")]
        public Guid? CarId { get; set; }
        [JsonIgnore]
        public Car? Car { get; set; }
        [ForeignKey("EntranceType")]
        public int EntranceTypeId { get; set; }
    }
}
