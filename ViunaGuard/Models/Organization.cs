using System.ComponentModel.DataAnnotations;

namespace ViunaGuard.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Entrance> Entrances { get; set; } = new List<Entrance>();
        public List<Door> Doors { get; set; } = new List<Door>();
    }
}
