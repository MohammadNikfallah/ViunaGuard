using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViunaGuard.Models;

namespace ViunaGuard.Dtos
{
    public class EmployeeShiftGetDto
    {
        public int EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public DoorGetDto? GuardDoor { get; set; }
        public int? ShiftMakerEmployeeId { get; set; }
        public EmployeeGetDto? ShiftMakerEmployee { get; set; } = null!;
    }
}
