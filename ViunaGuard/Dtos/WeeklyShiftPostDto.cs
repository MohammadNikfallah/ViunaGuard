using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViunaGuard.Models;

namespace ViunaGuard.Dtos
{
    public class WeeklyShiftPostDto
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int OrganizationId { get; set; }
        [Required]
        public int DayOfWeek { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime FinishTime { get; set; }
        public int? GuardDoorId { get; set; }
        public int? ShiftMakerEmployeeId { get; set; }
    }
}
