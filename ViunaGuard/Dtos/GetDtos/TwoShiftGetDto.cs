namespace ViunaGuard.Dtos;

public class TwoShiftGetDto
{
    public List<EmployeeShiftGetDto> TodayShifts { get; set; } = new();
    public List<EmployeeShiftGetDto> TomorrowShifts { get; set; } = new();
}