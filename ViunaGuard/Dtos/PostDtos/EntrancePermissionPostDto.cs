using System.ComponentModel.DataAnnotations;

namespace ViunaGuard.Dtos;

public class EntrancePermissionPostDto
{
    [Required]
    public int OrganizationId { get; set; }
    [Required]
    public DateTime StartValidityTime{ get; set; }
    [Required]
    public DateTime EndValidityTime{ get; set; }
    public int? CarId { get; set; }
    public int OrganizationPlaceId { get; set; }
}