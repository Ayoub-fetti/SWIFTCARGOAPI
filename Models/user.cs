using Microsoft.EntityFrameworkCore; 
using System.ComponentModel.DataAnnotations;

namespace SWIFTCARGOAPI.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required]
        [EmailAddress] 
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        public string Role { get; set; } = "User";
    }
}