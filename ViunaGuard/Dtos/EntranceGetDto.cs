using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViunaGuard.Models;

namespace ViunaGuard.Dtos
{
    public class EntranceGetDto
    {
        public int PersonId { get; set; }
        public Person? Person { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime Time { get; set; }
        public int? CarId { get; set; }
        public Car? Car { get; set; }
        public int? GuestCount { get; set; }
        public int GuardId { get; set; }
        public Employee Guard { get; set; } = null!;
        public int DoorId { get; set; }
        public Door Door { get; set; } = null!;
        public int? EntranceGroupId { get; set; }
        public EntranceGroup? EntranceGroup { get; set; }
        public int EntranceTypeId { get; set; }
        public int EnterOrExitId { get; set; }
    }
}
