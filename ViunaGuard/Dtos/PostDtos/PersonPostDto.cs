using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViunaGuard.Models;

namespace ViunaGuard.Dtos
{
    public class PersonPostDto
    {
        [Required]
        public string Id { get; set; } = null;
        [Required]
        public string NationalId { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
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
    }
}
