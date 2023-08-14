using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViunaGuard.Models;

namespace ViunaGuard.Dtos
{
    public class EntrancePermissionGetDto
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;
        public DateTime StartValidityTime { get; set; }
        public DateTime EndValidityTime { get; set; }
        public int? CarId { get; set; }
        public Car? Car { get; set; }
        public bool PermissionGranted { get; set; } = false;
        public List<SignedEntrancePermission> Signatures { get; set; } = new List<SignedEntrancePermission>();
    }
}
