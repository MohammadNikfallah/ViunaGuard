using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ViunaGuard.Models;

public class UserAccessRole
{
    [Key]
    public int Id { get; set; }
    [Required, ForeignKey("UserAccess")]
    public int UserAccessId { get; set; }
    [JsonIgnore]
    public UserAccess UserAccess { get; set; } = null!;
    public string? Name { get; set; }
}