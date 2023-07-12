using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViunaGuard.Models
{
    public class Person
    {
        [Key]
        public Guid Id { get; set; }
        [Required, ForeignKey("AdditionalInfo")]
        public Guid AdditionalInfoId { get; set; }
        public AdditionalInfo AdditionalInfo { get; set; }
        [Required]
        public int NationalId { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required string FirstName { get; set; }
        public string? Email { get; set; }
        public int? PhoneNumber { get; set; }
        public int? CellPhoneNumber { get; set; }
        public string? FathersName { get; set; }
        public int? GenderId { get; set; }
        public int? BirthPlaceId { get; set; }
        public string? BirthDay { get; set; }
        public int? CityOfResidenceId { get; set; }
        public int? EducationalDegreeId { get; set; }
        public List<Car> Cars { get; set; } = new List<Car>();
        public List<Employee> Jobs { get; set; } = new List<Employee>();
        public List<Entrance> Entrances { get; set;} = new List<Entrance>();
        public List<EntrancePermission> EntrancePermissions { get; set; } = new List<EntrancePermission>();
    }
}
