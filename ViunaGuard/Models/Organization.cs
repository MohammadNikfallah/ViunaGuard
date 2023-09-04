using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [JsonIgnore]
        public List<Employee> Employees { get; set; } = new List<Employee>();
        [JsonIgnore]
        public List<Entrance> Entrances { get; set; } = new List<Entrance>();
        [JsonIgnore]
        public List<Door> Doors { get; set; } = new List<Door>();
        [JsonIgnore]
        public List<Person> AreBanned { get; set; } = new List<Person>();
    }
}
