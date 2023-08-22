using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ViunaGuard.Models;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Dtos;

public class EmployeePostDto
{
    [Required]
    [Key]
    public int Id { get; set; }
    [Required]
    public int OrganizationId { get; set; }
    [Required]
    public int PersonId { get; set; }
    [Required]
    public int EmployeeTypeId{ get; set; }
    public string? PersonnelID { get; set; } = string.Empty;
    public int AuthorityLevelId { get; set; }
}