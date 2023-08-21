using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViunaGuard.Models;

namespace ViunaGuard.Dtos
{
    public class EntranceGroupPostDto
    {
        [Required]
        public int OrganizationId { get; set; }
        public int? DriverId { get; set; }
        public int? CarId { get; set; }
        public List<EntrancePostDto> Entrances { get; set; } = new List<EntrancePostDto>();
    }
}
