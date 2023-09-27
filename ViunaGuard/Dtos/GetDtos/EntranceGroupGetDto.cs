using System.Text.Json.Serialization;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Dtos;

public class EntranceGroupGetDto
{
    public int Id { get; set; }
    public Organization Organization { get; set; } = null!;
    public DateTime Time { get; set; }
    public int GuardId { get; set; }
    [JsonIgnore]
    public Employee Guard { get; set; } = null!;
    public DoorGetDto Door { get; set; } = null!;
    public EnterOrExit? EnterOrExit{ get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public bool Permitted { get; set; }
    public List<EntranceGetDto> Entrances { get; set; } = new();
}