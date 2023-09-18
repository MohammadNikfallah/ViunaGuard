using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ViunaGuard.Models;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Dtos;

public class EmployeePostDto
{
    [Required]
    public int OrganizationId { get; set; }
    [Required] 
    public string PersonId { get; set; } = null!;
    [Required]
    public int EmployeeTypeId{ get; set; }
    public string? PersonnelId { get; set; } = string.Empty;
    public int UserAccessRoleId { get; set; }
    public int WorkPlaceId { get; set; }
}