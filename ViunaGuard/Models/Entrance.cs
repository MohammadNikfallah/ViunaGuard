using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class Entrance
    {
        [Key, Required]
        public Guid Id { get; set; }
        [Required, ForeignKey("Person")]
        public Guid PersonId { get; set; }
        [JsonIgnore]
        public Person? Person { get; set; }
        [Required, ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [JsonIgnore]
        public Organization Organization { get; set; }
        [ForeignKey("Employee")]
        public Guid? EmployeeId { get; set; }
        [JsonIgnore]
        public Employee? Employee { get; set; }
        [Required]
        public DateTime EnterTime { get; set; }
        //
        public DateTime? ExitTime { get; set; }
        [ForeignKey("Car")]
        public Guid? CarId { get; set; }
        [JsonIgnore]
        public Car? Car { get; set; }
        public int? EnterGuestCount { get; set; }
        public int? ExitGuestCount { get; set; }
        [Required, ForeignKey("EnterGuard")]
        public Guid EnterGuardId { get; set; }
        [JsonIgnore]
        public Employee EnterGuard { get; set; }
        [ForeignKey("ExitGuard")]
        public Guid? ExitGuardId { get; set;}
        [JsonIgnore]
        public Employee? ExitGuard { get; set; }
        [Required]
        public int EnterDoorId { get; set; }
        [JsonIgnore]
        public Door EnterDoor { get; set; }
        public int? ExitDoorId { get; set; }
        [JsonIgnore]
        public Door ExitDoor { get; set; }
        public Guid? ReferenceEmployeeId { get; set; }
        //entrance type

    }
}
