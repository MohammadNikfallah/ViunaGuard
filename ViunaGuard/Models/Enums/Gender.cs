using System.ComponentModel.DataAnnotations;

namespace ViunaGuard.Models.Enums
{
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
