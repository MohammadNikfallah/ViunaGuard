using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class BlackList
    {
        [Key]
        public int Id { get; set; }
        [Required, ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [JsonIgnore]
        public Organization Organization { get; set; } = null!;
        [Required, ForeignKey("Person")]
        public int PersonId { get; set; }
        [JsonIgnore]
        public Person Person { get; set; } = null!;
    }
}
