using System.ComponentModel.DataAnnotations;

namespace ViunaGuard.Models.Enums
{
    public class CarBrand
    {
        [Key]
        public int Id { get; set; }
        public string Brand { get; set; } = null!;
    }
}
