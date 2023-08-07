using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ViunaGuard.Models;
using System.Text.Json.Serialization;

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
        public int? BirthPlaceId { get; set; }
        public DateOnly? BirthDay { get; set; }
        public int? CityOfResidenceId { get; set; }
        public int? EducationalDegreeId { get; set; }
        public int? MilitaryServiceStatsCode { get; set; }
        public int? NationalityId { get; set; }
        public int? ReligionId { get; set; }
        public int? MaritalStatusId { get; set; }
        public List<Car>? Cars { get; set; }
        public List<Employee>? Jobs { get; set; }
        public List<Entrance>? Entrances { get; set; }
        public List<EntrancePermission>? EntrancePermissions { get; set; }
        public List<Organization>? BannedFrom { get; set; }
    }
}
