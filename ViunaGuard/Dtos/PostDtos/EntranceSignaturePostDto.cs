using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ViunaGuard.Dtos;

public class EntranceSignaturePostDto
{
    [Required]
    public int OrganizationId { get; set; }
    [Required]
    public int AuthorityLevel { get; set; }
    [Required]
    public int SignedByEmployeeId { get; set; }
    [Required]
    public int EntrancePermissionId { get; set; }
    public DateTime Time { get; set; }
}