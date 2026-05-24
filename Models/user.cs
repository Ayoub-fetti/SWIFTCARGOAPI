using System.ComponentModel.DataAnnotations;
namespace SWIFTCARGOAPI.Models;

public class User
{
    public int Id { get; set; }


    [Required]
    [StringLength(50)]
    public required string Username { get; set; }

    [Required]
    public required string PasswordHash { get; set; }

    public string Role { get; set; } = "User";
}
