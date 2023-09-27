using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class EntrancePermission
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required, ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [JsonIgnore]
        public Organization Organization { get; set; } = null!;
        [Required, ForeignKey("Person")]
        public string PersonId { get; set; } = null!;
        public Person Person { get; set; } = null!;
        [Required]
        public DateTime StartValidityTime{ get; set; }
        [Required]
        public DateTime EndValidityTime{ get; set; }
        [ForeignKey("Car")]
        public int? CarId { get; set; }
        [JsonIgnore]
        public Car? Car { get; set; }
        public bool PermissionGranted { get; set; }
        public bool Revoked { get; set; }
        [ForeignKey("OrganizationPlace")]
        public int OrganizationPlaceId { get; set; }
        [JsonIgnore]
        public OrganizationPlace? OrganizationPlace { get; set; }
        [JsonIgnore]
        public List<SignedEntrancePermission> Signatures { get; set; } = new();
    }
}
