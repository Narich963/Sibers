using CSharpFunctionalExtensions;
using Sibers.Core.Enums;
using Sibers.Services.DTO;

namespace Sibers.Services.Interfaces;

public interface IProjectService
{
    Task<Result<IEnumerable<ProjectDTO>>> GetAllAsync();
    Task<IEnumerable<ProjectDTO>> GetPagedAsync(int page,
        string sortField,
        bool ascending,
        string? name,
        Priority? priority,
        DateTime? startDate,
        DateTime? endDate,
        int pageSize);
    Task<Result<ProjectDTO>> GetAsync(int id);
    Task<Result<ProjectDTO>> CreateAsync(ProjectDTO project);
    Task<Result<ProjectDTO>> Update(ProjectDTO project);
    Task<Result> Delete(int? id);
    Task<int> Count();
    Task<Result> SetEmployee(int? userId, int? projectId, bool isManager);
    Task<Result> RemoveEmployee(int? userId, int? projectId);

}
