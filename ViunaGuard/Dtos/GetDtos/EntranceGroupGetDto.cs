using System.Text.Json.Serialization;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Dtos;

public class EntranceGroupGetDto
{
    public int Id { get; set; }
    [JsonIgnore]
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    public DateTime Time { get; set; }
    public int GuardId { get; set; }
    [JsonIgnore]
    public Employee Guard { get; set; } = null!;
    [JsonIgnore]
    public int DoorId { get; set; }
    public Door Door { get; set; } = null!;
    [JsonIgnore]
    public int EnterOrExitId { get; set; }
    public EnterOrExit? EnterOrExit{ get; set; }
    public List<EntranceGetDto> Entrances { get; set; } = new();
}