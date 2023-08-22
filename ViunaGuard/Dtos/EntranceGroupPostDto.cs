using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViunaGuard.Models;

namespace ViunaGuard.Dtos
{
    public class EntranceGroupPostDto
    {
        public List<EntrancePostDto> Entrances { get; set; } = new();
    }
}
