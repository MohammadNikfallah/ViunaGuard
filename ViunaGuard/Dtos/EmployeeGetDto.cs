using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Dtos;

public class EmployeeGetDto
{
    public int Id { get; set; }
    [JsonIgnore]
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    [JsonIgnore]
    public int EmployeeTypeId{ get; set; }
    public EmployeeType? EmployeeType { get; set; }
    public string? PersonnelID { get; set; } = string.Empty;
    [JsonIgnore]
    public int AuthorityLevelId { get; set; }
    public Authority Authority { get; set; } = null!;
}