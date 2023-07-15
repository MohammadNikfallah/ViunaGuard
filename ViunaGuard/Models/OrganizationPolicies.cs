using System.ComponentModel.DataAnnotations;

namespace ViunaGuard.Models
{
    public class OrganizationPolicie
    {
        [Key]
        public int OrganizationId { get; set; }
        public bool CheckGuests { get; set; } = false;
        public bool CheckUnregisteredGuests { get; set; } = false;
        public bool CheckCarsOnConferenceMode { get; set; } = false;
        public bool CheckpeopleOnConferenceMode { get; set; } = false;

    }
}
