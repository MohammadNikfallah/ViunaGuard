using Microsoft.Build.Framework;

namespace ViunaGuard.Dtos;

public class PeriodicShiftPostDto
{
    [Required]
    public int EmployeeId { get; set; }
    [Required]
    public int PeriodDayRange { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime FinishTime { get; set; }
    public int? GuardDoorId { get; set; }
    public int WorkPlaceId { get; set; }
}