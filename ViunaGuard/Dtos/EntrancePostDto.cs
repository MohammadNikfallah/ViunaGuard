using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViunaGuard.Dtos
{
    public class EntrancePostDto
    {
        [Required]
        public int PersonId { get; set; }
        [Required]
        public int OrganizationId { get; set; }
        [Required]
        public DateTime Time { get; set; }
        public int? CarId { get; set; }
        public int? GuestCount { get; set; }
        [Required]
        public int GuardId { get; set; }
        [Required]
        public int DoorId { get; set; }
        public int EntranceTypeId { get; set; }
        public int EnterOrExitId { get; set; }
    }
}
