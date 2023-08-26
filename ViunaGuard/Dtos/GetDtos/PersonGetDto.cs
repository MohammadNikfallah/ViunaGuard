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
        public PersonAdditionalInfo? PersonAdditionalInfo { get; set; }
        public string NationalId { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CellPhoneNumber { get; set; }
        public string? FathersName { get; set; }
        public string? Gender { get; set; }
        public string? BirthPlaceCity { get; set; }
        public DateOnly? BirthDay { get; set; }
        public string? CityOfResidence { get; set; }
        public string? EducationalDegree { get; set; }
        public string? MilitaryServiceStatus { get; set; }
        public string? Nationality { get; set; }
        public string? Religion { get; set; }
        public string? MaritalStatus { get; set; }
        public List<Car> Cars { get; set; } = new();
        public List<Employee> Jobs { get; set; } = new();
        public List<Entrance> Entrances { get; set; } = new();
        public List<EntrancePermission> EntrancePermissions { get; set; } = new();
        public List<Organization> BannedFrom { get; set; } = new();
    }
}
