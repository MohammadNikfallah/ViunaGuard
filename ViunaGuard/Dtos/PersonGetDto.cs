using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ViunaGuard.Models;
using System.Text.Json.Serialization;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Dtos
{
    public class PersonGetDto
    {
        public int Id { get; set; }
        public int PersonAdditionalInfoId { get; set; }
        public PersonAdditionalInfo? PersonAdditionalInfo { get; set; }
        public string NationalId { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CellPhoneNumber { get; set; }
        public string? FathersName { get; set; }
        public int? GenderId { get; set; }
        public Gender? Gender { get; set; }
        public int? BirthPlaceCityId { get; set; }
        public City? BirthPlaceCity { get; set; }
        public DateOnly? BirthDay { get; set; }
        public int? CityOfResidenceId { get; set; }
        public City? CityOfResidence { get; set; }
        public int? EducationalDegreeId { get; set; }
        public EducationalDegree? EducationalDegree { get; set; }
        public int? MilitaryServiceStatusId { get; set; }
        public MilitaryServiceStatus? MilitaryServiceStatus { get; set; }
        public int? NationalityId { get; set; }
        public Nationality? Nationality { get; set; }
        public int? ReligionId { get; set; }
        public Religion? Religion { get; set; }
        public int? MaritalStatusId { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public List<Car> Cars { get; set; } = new List<Car>();
        public List<Employee> Jobs { get; set; } = new List<Employee>();
        public List<Entrance> Entrances { get; set; } = new List<Entrance>();
        public List<EntrancePermission> EntrancePermissions { get; set; } = new List<EntrancePermission>();
        public List<Organization> BannedFrom { get; set; } = new List<Organization>();
    }
}
