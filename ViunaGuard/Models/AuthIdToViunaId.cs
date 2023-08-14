using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ViunaGuard.Models
{
    public class AuthIdToViunaId
    {
        public int ViunaUserId { get; set; }
        [Key]
        public Guid AuthId { get; set; } 
    }
}
