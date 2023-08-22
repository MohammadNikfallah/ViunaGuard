using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViunaGuard.Models;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Dtos
{
    public class EntranceGroupPostDto
    {
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public int DoorId { get; set; }
        [Required]
        public int EnterOrExitId { get; set; }
        [Required]
        public List<EntrancePostDto> Entrances { get; set; } = new();
    }
}
