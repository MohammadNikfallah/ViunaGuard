using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Dtos;

public class EmployeeGetDto
{
    public int Id { get; set; }
    public Organization Organization { get; set; } = null!;
    public EmployeeType? EmployeeType { get; set; }
    public string? PersonnelID { get; set; } = string.Empty;
    public OrganizationPlace? WorkPlace { get; set; }
}