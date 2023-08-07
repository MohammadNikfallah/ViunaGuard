using System.ComponentModel.DataAnnotations;

namespace ViunaGuard.Models.Enums
{
    public class CarModel
    {
        [Key]
        public int Id { get; set; }
        public string Model { get; set; } = null!;
    }
}
