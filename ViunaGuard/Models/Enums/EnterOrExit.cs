using System.ComponentModel.DataAnnotations;

namespace ViunaGuard.Models.Enums
{
    public class EnterOrExit
    {

        [Key]
        public int Id { get; set; }
        public string Type { get; set; } = null!;
    }
}
