using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class Person
    {
        [Key] public string Id { get; set; } = null!;
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
        [ForeignKey("Gender")]
        public int? GenderId { get; set; }
        public Gender? Gender { get; set; }
        [ForeignKey("BirthPlaceCity")]
        public int? BirthPlaceCityId { get; set; }
        public City? BirthPlaceCity { get; set; }
        public DateOnly? BirthDay { get; set; }
        [ForeignKey("CityOfResidence")]
        public int? CityOfResidenceId { get; set; }
        public City? CityOfResidence { get; set; }
        [ForeignKey("EducationalDegree")]
        public int? EducationalDegreeId { get; set; }
        public EducationalDegree? EducationalDegree { get; set; }
        [ForeignKey("MilitaryServiceStatus")]
        public int? MilitaryServiceStatusId { get; set; }
        public MilitaryServiceStatus? MilitaryServiceStatus { get; set; }
        [ForeignKey("Nationality")]
        public int? NationalityId { get; set; }
        public Nationality? Nationality { get; set; }
        [ForeignKey("Religion")]
        public int? ReligionId { get; set; }
        public Religion? Religion { get; set; }
        [ForeignKey("MaritalStatus")]
        public int? MaritalStatusId { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        [JsonIgnore]
        public List<Car> Cars { get; set; } = new();
        [JsonIgnore]
        public List<Employee> Jobs { get; set; } = new();
        [JsonIgnore]
        public List<Entrance> Entrances { get; set; } = new();
        [JsonIgnore]
        public List<EntrancePermission> EntrancePermissions { get; set; } = new();
        [JsonIgnore]
        public List<Organization> BannedFrom { get; set; } = new();
        //TODO signed implementation
        public bool Signed { get; set; } = false;
    }
}
