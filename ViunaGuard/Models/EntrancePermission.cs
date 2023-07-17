using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class EntrancePermission
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required, ForeignKey("Person")]
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
        [Required]
        public DateTime StartValidityTime{ get; set; }
        [Required]
        public DateTime EndValidityTime{ get; set; }
        [ForeignKey("Car")]
        public Guid? CarId { get; set; }
        [JsonIgnore]
        public Car Car { get; set; }
        [Required]
        public bool PermissionGranted { get; set; }
        [ForeignKey("PermissionGranter")]
        public Guid? PermissionGranterEmployeeId { get; set; }
        [JsonIgnore]
        public Employee PermissionGranter { get; set; }
        //inviter

    }
}
