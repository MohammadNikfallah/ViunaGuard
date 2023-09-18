using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Models
{
    public class Employee
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [JsonIgnore]
        public Organization Organization { get; set; } = null!;
        [Required, ForeignKey("Person")]
        public string PersonId { get; set; } = null!;
        [JsonIgnore]
        public Person Person { get; set; } = null!;
        [Required,ForeignKey("EmployeeType")]
        public int EmployeeTypeId{ get; set; }
        public EmployeeType? EmployeeType { get; set; }
        public string? PersonnelId { get; set; }
        [ForeignKey("UserAccessRole")]
        public int UserAccessRoleId { get; set; }
        [JsonIgnore]
        public UserAccessRole UserAccessRole { get; set; } = null!;
        public int WorkPlaceId { get; set; }
        [JsonIgnore]
        public OrganizationPlace? WorkPlace { get; set; }
    }
}
