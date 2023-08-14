using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViunaGuard.Models;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Dtos
{
    public class EntranceGetDto
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int PersonId { get; set; }
        public Person? Person { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;
        public DateTime Time { get; set; }
        [JsonIgnore]
        public int? CarId { get; set; }
        public Car? Car { get; set; }
        public int? GuestCount { get; set; }
        [JsonIgnore]
        public int GuardId { get; set; }
        public Employee Guard { get; set; } = null!;
        [JsonIgnore]
        public int DoorId { get; set; }
        public Door Door { get; set; } = null!;
        [JsonIgnore]
        public int? EntranceGroupId { get; set; }
        public EntranceGroup? EntranceGroup { get; set; }
        [JsonIgnore]
        public int EntranceTypeId { get; set; }
        public EntranceType? EntranceType { get; set; }
        [JsonIgnore]
        public int EnterOrExitId { get; set; }
        public EnterOrExit? EnterOrExit { get; set; }
    }
}
