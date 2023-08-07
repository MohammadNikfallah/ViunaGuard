using System.ComponentModel.DataAnnotations;

namespace ViunaGuard.Models.Enums
{
    public class MilitaryServiceStatus
    {
        [Key]
        public int Id { get; set; }
        public string Stat { get; set; } = null!;
    }
}
