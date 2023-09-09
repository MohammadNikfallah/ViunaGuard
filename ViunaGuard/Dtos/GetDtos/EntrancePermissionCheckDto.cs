using System.Text.Json.Serialization;

namespace ViunaGuard.Dtos;

public class EntrancePermissionCheckDto
{
    public PersonGetDto Person { get; set; } = null!;
    public EmployeeGetDto? Job { get; set; }
    public TwoShiftGetDto? Shifts { get; set; }
    public List<EntrancePermissionGetDto> EntrancePermissions { get; set; } = new();
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public bool DoesHavePermission { get; set; }
}