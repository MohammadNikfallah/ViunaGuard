using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViunaGuard.Models
{
    public class UserAccess
    {
        [Key]
        public int Id { get; set; }
        public int AuthorityLevel { get; set; }
        public bool CanChangeShifts { get; set; }
        public bool CanBringGuests { get; set; }
        public bool CanInviteGuests { get; set; }
        public bool AlwaysHaveEntrancePermission { get; set; }
        public bool CanSeeOtherGuardsEntrances { get; set; }
        public bool CanSeeOtherDoorsEntrances { get; set; }
        public bool CanAddEmployees { get; set; }
        public bool CanSignEntrancePermissions { get; set; }
        public bool CanRevokeEntrancePermissions { get; set; }
        public bool CanSeeEntrancePermissions { get; set; }
        public bool CanSignVisitedPlace { get; set; }
    }
}
