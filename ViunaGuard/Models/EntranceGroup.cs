using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Models
{
    public class EntranceGroup
    {
        [Key]
        public int Id { get; set; }
        [Required, ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [JsonIgnore]
        public Organization Organization { get; set; } = null!;
        [Required]
        public DateTime Time { get; set; }
        [Required, ForeignKey("Guard")]
        public int GuardId { get; set; }
        [JsonIgnore]
        public Employee Guard { get; set; } = null!;
        [Required, ForeignKey("Door")]
        public int DoorId { get; set; }
        [JsonIgnore]
        public Door Door { get; set; } = null!;
        [ForeignKey("EnterOrExit")]
        public int EnterOrExitId { get; set; }
        public EnterOrExit? EnterOrExit{ get; set; }
        [JsonIgnore]
        public List<Entrance> Entrances { get; set; } = new();
    }
}
