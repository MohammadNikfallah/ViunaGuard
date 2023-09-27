using System.Text.Json.Serialization;

namespace ViunaGuard.Dtos;

public class CheckExitPermissionDto
{
    public PersonGetDto Person { get; set; } = null!;
    public EmployeeGetDto? Job { get; set; }
    public List<EntrancePermissionGetDto> EntrancePermissions { get; set; } = new();
    [JsonIgnore]
    public List<VisitedPlace> VisitedPlaces { get; set; } = new();
    public bool DoesHavePermission { get; set; }
}