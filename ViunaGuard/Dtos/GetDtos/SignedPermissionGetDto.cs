using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ViunaGuard.Dtos;

public class SignedPermissionGetDto
{
    public int Id { get; set; }
    public int AuthorityLevel { get; set; }
    public int SignedByEmployeeId { get; set; }
    public int EntrancePermissionId { get; set; }
    public DateTime Time { get; set; }
}