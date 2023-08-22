using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViunaGuard.Models;

namespace ViunaGuard.Dtos
{
    public class EmployeeShiftGetDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int OrganizationId { get; set; }
        public Organization? Organization { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public int? GuardDoorId { get; set; }
        public Door? GuardDoor { get; set; }
        public int? ShiftMakerEmployeeId { get; set; }
        public Employee? ShiftMakerEmployee { get; set; } = null!;
    }
}
