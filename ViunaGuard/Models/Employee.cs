using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class Employee
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }//unique with personnel id
        [JsonIgnore]
        public Organization Organization { get; set; } = null!;
        [Required, ForeignKey("Person")]
        public int PersonId { get; set; }
        [JsonIgnore]
        public Person Person { get; set; } = null!;
        [Required]
        public int EmployeeTypeId{ get; set; }
        public string? PersonnelID { get; set; } = string.Empty;
        [ForeignKey("Authority")]
        public int AuthorityLevelId { get; set; }
        [JsonIgnore]
        public Authority Authority { get; set; } = null!;
        [JsonIgnore, InverseProperty("Employee")]
        public List<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();
    }
}
