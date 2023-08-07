using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class Authority
    {
        [Required, ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [JsonIgnore]
        public Organization Organization { get; set; } = null!;
        [Key]
        public int Id { get; set; }
        [Required]
        public int AuthorityLevel { get; set; }
        [Required]
        public string AuthorityLevelName { get; set; } = null!;
    }
}
