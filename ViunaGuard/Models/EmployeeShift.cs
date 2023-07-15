using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class EmployeeShift
    {
        [Key]
        public Guid Id { get; set; }
        [Required, ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }
        [JsonIgnore]
        public Employee? Employee { get; set; }
        [Required, ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [JsonIgnore]
        public Organization Organization { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime FinishTime { get; set; }
        [ForeignKey("GuardDoor")]
        public int? GuardDoorId { get; set; }
        [JsonIgnore]
        public Door? GuardDoor { get; set; }
        [ForeignKey("ShiftMakerEmployee")]
        public Guid ShiftMakerEmployeeId { get; set; }
        [JsonIgnore]
        public Employee ShiftMakerEmployee { get; set; }


    }
}
