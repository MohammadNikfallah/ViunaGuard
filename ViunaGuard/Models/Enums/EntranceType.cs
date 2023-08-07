using System.ComponentModel.DataAnnotations;

namespace ViunaGuard.Models.Enums
{
    public class EntranceType
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; } = null!;
    }
}
