using System.ComponentModel.DataAnnotations;

namespace ViunaGuard.Models
{
    public class EntrancePolicie
    {
        [Key]
        public int DoorId { get; set; }
        public bool CheckCars { get; set; } = false;
        public bool CheckPoeple { get; set; } = false;
    }
}
