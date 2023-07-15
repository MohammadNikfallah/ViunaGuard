using System.ComponentModel.DataAnnotations;

namespace ViunaGuard.Models
{
    public class UserAccess
    {
        [Key]
        public Guid Id { get; set; }
        public bool CanChangeShifts { get; set; } = false;
        public bool CanBringGuests { get; set; } = false;
        public bool CanInviteGuests { get; set; } = false;
        public bool AlwaysHaveEntrancePermission { get; set; } = false;

    }
}
