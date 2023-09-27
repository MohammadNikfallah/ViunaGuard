using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ViunaGuard.Models;

public class VisitedPlace
{
    [Key]
    public int Id { get; set; }
    [Required ,ForeignKey("OrganizationPlace")]
    public int OrganizationPlaceId { get; set; }
    [JsonIgnore]
    public OrganizationPlace? OrganizationPlace { get; set; }
    [Required ,ForeignKey("Person")]
    public string PersonId { get; set; } = string.Empty;
    [JsonIgnore]
    public Person? Person { get; set; }
    [Required]
    public DateTime VisitTime { get; set; }
    [ForeignKey("SignatoryEmployee")]
    public int SignatoryEmployeeId { get; set; }
    [JsonIgnore]
    public Employee? SignatoryEmployee { get; set; }
}