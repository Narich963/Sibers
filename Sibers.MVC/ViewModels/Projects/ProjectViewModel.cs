using Sibers.Core.Enums;
using Sibers.MVC.ViewModels.Users;
using Sibers.Services.DTO;

namespace Sibers.MVC.ViewModels.Projects;

public class ProjectViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CustomerCompanyName { get; set; }
    public string ExecutorCompanyName { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Priority Priority { get; set; }

    public int? ManagerUserId { get; set; }
    public UserViewModel? Manager { get; set; }

    public List<UserViewModel> Employees { get; set; }
}
