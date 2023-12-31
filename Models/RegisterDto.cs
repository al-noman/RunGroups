using System.ComponentModel.DataAnnotations;

namespace RunGroups.Models;

public class RegisterDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage="Passwords do not match")]
    public string ConfirmPassword { get; set; }
}