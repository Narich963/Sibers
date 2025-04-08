namespace Sibers.Core.Entities;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string CustomerCompanyName { get; set; }
    public string ExecutorCompanyName { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Priority { get; set; }

    public int ManagerUserId { get; set; }
    public User Manager { get; set; }

    public List<User> Employees { get; set; }
    
    public Project()
    {
        Employees = new();    
    }
}
