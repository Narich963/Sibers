using Microsoft.AspNetCore.Identity;

namespace Sibers.Core.Entities;

public class User : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Avatar { get; set; } = "https://img.freepik.com/premium-vector/default-avatar-profile-icon-social-media-user-image-gray-avatar-icon-blank-profile-silhouette-vector-illustration_561158-3467.jpg";

    public List<Project> Projects { get; set; }
    public User()
    {
        Projects = new();
    }
}
