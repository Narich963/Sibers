namespace Sibers.Services.DTO;

/// <summary>
/// Data transfer object of User entity
/// </summary>
public class UserDTO
{
    public string Email { get; set; }

    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }

    public string Password { get; set; }
}
