using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class EmployeeShiftPeriodicMonthly
    {
        [Key]
        public int Id { get; set; }
        [Required, ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        [JsonIgnore]
        public Employee? Employee { get; set; }
        [Required, ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [JsonIgnore]
        public Organization Organization { get; set; } = null!;
        [Required]
        public int DayOfMonth { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime FinishTime { get; set; }
        [ForeignKey("GuardDoor")]
        public int? GuardDoorId { get; set; }
        [JsonIgnore]
        public Door? GuardDoor { get; set; }
        [ForeignKey("ShiftMakerEmployee")]
        public int? ShiftMakerEmployeeId { get; set; }
        [JsonIgnore]
        public Employee? ShiftMakerEmployee { get; set; } = null!;
    }
}
