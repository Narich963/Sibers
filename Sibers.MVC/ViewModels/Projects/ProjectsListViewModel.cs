using Sibers.Core.Entities;
using Sibers.Core.Enums;

namespace Sibers.MVC.ViewModels.Projects;

public class ProjectsListViewModel
{
    public IEnumerable<Project> Projects { get; set; } = new List<Project>();

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public string SortField { get; set; }
    public bool Ascending { get; set; }

    public string? NameFilter { get; set; }
    public Priority? PriorityFilter { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
}
