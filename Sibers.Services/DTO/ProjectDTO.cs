using Sibers.Core.Enums;

namespace Sibers.Services.DTO;

public class ProjectDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CustomerCompanyName { get; set; }
    public string ExecutorCompanyName { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Priority Priority { get; set; }

    public int? ManagerUserId { get; set; }
    public UserDTO? Manager { get; set; }

    public List<UserDTO> Employees { get; set; }
}
