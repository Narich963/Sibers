namespace Sibers.Services.DTO;

/// <summary>
/// Data transfer object of User entity
/// </summary>
public class UserDTO
{
    public int Id { get; set; }
    public string Email { get; set; }

    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Avatar { get; set; }

    public string Password { get; set; }

    public List<ProjectDTO> Projects { get; set; }
}
