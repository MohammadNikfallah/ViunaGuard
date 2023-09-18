using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViunaGuard.Models;

public class OrganizationPlace
{
    public int Id { get; set; }
    [ForeignKey("Organization")]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public Organization? Organization { get; set; }
    [Required] 
    public string Name { get; set; } = null!;
    [JsonIgnore] 
    public List<Employee> Employees { get; set; } = new();
    [JsonIgnore] 
    public List<EntrancePermission> EntrancePermissions { get; set; } = new();
}