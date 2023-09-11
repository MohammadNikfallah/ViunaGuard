using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViunaGuard.Models;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Dtos
{
    public class EntranceGetDto
    {
        public int Id { get; set; }
        [JsonIgnore] 
        public string PersonId { get; set; } = null!;
        public PersonForEntranceGetDto? Person { get; set; }
        [JsonIgnore]
        public int? CarId { get; set; }
        public Car? Car { get; set; }
        public int? GuestCount { get; set; }
        public bool IsDriver { get; set; } = false;
        public bool Permitted { get; set; }
    }
}
