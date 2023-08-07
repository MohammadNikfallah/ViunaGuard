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
        [Required, ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [JsonIgnore]
        public Organization Organization { get; set; } = null!;
        [ForeignKey("Employee")]
        public int? EmployeeId { get; set; }
        [JsonIgnore]
        public Employee? Employee { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [ForeignKey("Car")]
        public int? CarId { get; set; }
        [JsonIgnore]
        public Car? Car { get; set; }
        public int? GuestCount { get; set; }
        [Required, ForeignKey("Guard")]
        public int GuardId { get; set; }
        [JsonIgnore]
        public Employee Guard { get; set; } = null!;
        [Required, ForeignKey("Door")]
        public int DoorId { get; set; }
        [JsonIgnore]
        public Door Door { get; set; } = null!;
        [ForeignKey("EntranceGroup")]
        public int? EntranceGroupId { get; set; }
        [JsonIgnore]
        public EntranceGroup? EntranceGroup { get; set; }
        [ForeignKey("EntranceType")]
        public int EntranceTypeId { get; set; }
        public EntranceType? EntranceType { get; set; }
        [ForeignKey("EnterOrExit")]
        public int EnterOrExitId { get; set; }
        public EnterOrExit? EnterOrExit{ get; set; }
    }
}
