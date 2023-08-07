using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required, ForeignKey("PersonAdditionalInfo")]
        public int PersonAdditionalInfoId { get; set; }
        [JsonIgnore]
        public PersonAdditionalInfo? PersonAdditionalInfo { get; set; }
        [Required, StringLength(10)]
        public string NationalId { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? CellPhoneNumber { get; set; }
        public string? FathersName { get; set; }
        public int? GenderId { get; set; }
        public int? BirthPlaceCityId { get; set; }
        public DateOnly? BirthDay { get; set; }
        public int? CityOfResidenceId { get; set; }
        public int? EducationalDegreeId { get; set; }
        public int? MilitaryServiceStatusId { get; set; }
        public int? NationalityId { get; set; }
        public int? ReligionId { get; set; }
        public int? MaritalStatusId { get; set; }
        [JsonIgnore]
        public List<Car> Cars { get; set; } = new List<Car>();
        [JsonIgnore]
        public List<Employee> Jobs { get; set; } = new List<Employee>();
        [JsonIgnore]
        public List<Entrance> Entrances { get; set; } = new List<Entrance>();
        [JsonIgnore]
        public List<EntrancePermission> EntrancePermissions { get; set; } = new List<EntrancePermission>();
        [JsonIgnore]
        public List<Organization> BannedFrom { get; set; } = new List<Organization>();
    }
}
