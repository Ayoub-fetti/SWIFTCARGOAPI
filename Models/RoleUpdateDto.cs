using System.ComponentModel.DataAnnotations;

namespace SWIFTCARGOAPI.Models
{
    public class RoleUpdateDto
    {
        [Required]
        public required string Role { get; set; }
    }
}