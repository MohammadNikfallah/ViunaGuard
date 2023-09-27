namespace ViunaGuard.Dtos;

public class PeriodicShiftGetDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int PeriodDayRange { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime FinishTime { get; set; }
    public Door? GuardDoor { get; set; }
    public EmployeeGetDto? ShiftMakerEmployee { get; set; } = null!;
    public OrganizationPlace? WorkPlace { get; set; }
}