using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;
using ViunaGuard.Models;

namespace ViunaGuard.Dtos
{
    public class ShiftPostDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime FinishTime { get; set; }
        public int? GuardDoorId { get; set; }
        public int WorkPlaceId { get; set; }
    }
}
