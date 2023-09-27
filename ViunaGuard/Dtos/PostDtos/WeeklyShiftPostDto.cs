using Microsoft.Build.Framework;

namespace ViunaGuard.Dtos;

public class WeeklyShiftPostDto
{
    [Required]
    public int EmployeeId { get; set; }
    [Required]
    public int DayOfWeek { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime FinishTime { get; set; }
    public int? GuardDoorId { get; set; }
    public int WorkPlaceId { get; set; }
}