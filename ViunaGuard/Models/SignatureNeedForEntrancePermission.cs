using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class SignatureNeedForEntrancePermission
    {
        [Key]
        public int Id { get; set; }
        [Required, ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [JsonIgnore]
        public Organization? Organization { get; set; }
        [Required, ForeignKey("MinAuthority")]
        public int MinAuthorityId { get; set; }
        [JsonIgnore]
        public Authority? MinAuthority { get; set; }
    }
}
