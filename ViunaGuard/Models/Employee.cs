using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models
{
    public class Employee
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }//unique with personnel id
        [JsonIgnore]
        public Organization Organization { get; set; }
        [Required, ForeignKey("Person")]
        public Guid PersonId { get; set; }
        [JsonIgnore]
        public Person Person { get; set; }
        [Required]
        public int EmployeeTypeId{ get; set; }
        public string? PersonnelID { get; set; } = string.Empty;
        [JsonIgnore]
        public List<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();

    }
}
