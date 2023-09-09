namespace ViunaGuard.Dtos;

public class EntrancePermissionCheckDto
{
    public PersonGetDto Person { get; set; } = null!;
    public EmployeeGetDto? Job { get; set; }
    public TwoShiftGetDto? Shifts { get; set; }
    public List<EntrancePermissionGetDto> EntrancePermissions { get; set; } = new();
    public bool DoesHavePermission { get; set; }
}