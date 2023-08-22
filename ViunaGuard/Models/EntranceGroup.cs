using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class EntranceGroup
    {
        [Key]
        public int Id { get; set; }
        [Required, ForeignKey("Organization")]
        public List<Entrance> Entrances { get; set; } = new();
    }
}
