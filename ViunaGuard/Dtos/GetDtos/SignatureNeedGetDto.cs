using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViunaGuard.Models;

namespace ViunaGuard.Dtos
{
    public class SignatureNeedGetDto
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        [JsonIgnore]
        public int MinAuthorityLevel { get; set; }
    }
}
