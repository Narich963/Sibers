using System.ComponentModel.DataAnnotations;

namespace Sibers.MVC.ViewModels.Users;

public class CreateUserViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string FirstName { get; set; }
    [Required]
    public string MiddleName { get; set; }
    [Required]
    public string LastName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password are not equal")]
    public string ConfirmPassword { get; set; }
}
