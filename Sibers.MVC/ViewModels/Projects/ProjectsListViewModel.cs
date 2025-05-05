using Sibers.Core.Entities;
using Sibers.Core.Enums;
using Sibers.Services.DTO;

namespace Sibers.MVC.ViewModels.Projects;

public class ProjectsListViewModel
{
    public IEnumerable<ProjectDTO> Projects { get; set; } = new List<ProjectDTO>();

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public string SortField { get; set; }
    public bool Ascending { get; set; }

    public string? NameFilter { get; set; }
    public Priority? PriorityFilter { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
}
