using System.ComponentModel.DataAnnotations;

namespace ViunaGuard.Models.Enums
{
    public class EducationalDegree
    {
        [Key]
        public int Id { get; set; }
        public string Degree { get; set; } = null!;
    }
}
