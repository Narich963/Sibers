using CSharpFunctionalExtensions;
using Sibers.Core.Entities;
using Sibers.Core.Enums;

namespace Sibers.Services.Interfaces;

public interface IProjectService
{
    Task<Result<IEnumerable<Project>>> GetAllAsync();
    Task<IEnumerable<Project>> GetPagedAsync(int page,
        string sortField,
        bool ascending,
        string? name,
        Priority? priority,
        DateTime? startDate,
        DateTime? endDate,
        int pageSize);
    Task<Result<Project>> GetAsync(int id);
    Task<Result<Project>> CreateAsync(Project project);
    Task<Result<Project>> Update(Project project);
    Task<Result> Delete(int? id);
    Task<int> Count();
    Task<Result> SetEmployee(int? userId, int? projectId, bool isManager);
    Task<Result> RemoveEmployee(int? userId, int? projectId);

}
