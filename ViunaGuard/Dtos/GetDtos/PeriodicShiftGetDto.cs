namespace ViunaGuard.Dtos;

public class PeriodicShiftGetDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    public int PeriodDayRange { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime FinishTime { get; set; }
    public Door? GuardDoor { get; set; }
    public EmployeeGetDto? ShiftMakerEmployee { get; set; } = null!;
}