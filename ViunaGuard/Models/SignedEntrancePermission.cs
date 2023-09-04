using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class SignedEntrancePermission
    {
        [Key]
        public int Id { get; set; }
        [Required, ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [JsonIgnore]
        public Organization? Organization { get; set; }
        [Required]
        public int AuthorityLevel { get; set; }
        [Required, ForeignKey("SigningEmployee")]
        public int SignedByEmployeeId { get; set; }
        [JsonIgnore]
        public Employee? SigningEmployee { get; set; }
        [Required, ForeignKey("EntrancePermission")]
        public int EntrancePermissionId { get; set; }
        [JsonIgnore]
        public EntrancePermission EntrancePermission { get; set; }

        public DateTime Time { get; set; }
    }
}
