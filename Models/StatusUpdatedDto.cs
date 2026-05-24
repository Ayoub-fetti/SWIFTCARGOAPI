using System.ComponentModel.DataAnnotations;

namespace SWIFTCARGOAPI.Models
{
    public class StatusUpdateDto
    {
        [Required]
        public required string Status { get; set; }
    }
}