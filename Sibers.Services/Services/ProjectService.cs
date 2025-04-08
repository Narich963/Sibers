using CSharpFunctionalExtensions;
using Sibers.Core.Entities;
using Sibers.Core.Interfaces;

namespace Sibers.Services.Services;

public class ProjectService
{
    private readonly IUnitOfWork _uow;
    public ProjectService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<IEnumerable<Project>>> GetAllAsync() => Result.Success(await _uow.ProjectManager.GetAllAsync());

    public async Task<Result<Project>> GetAsync(int id)
    {
        var project = await _uow.ProjectManager.Get(id);
        if (project != null)
            return Result.Success(project);
        return Result.Failure<Project>($"No project with Id = {id} was found.");
    }

    public async Task<Result<Project>> CreateAsync(Project project)
    {
        if (project != null)
        {
            await _uow.ProjectManager.Create(project);
            await _uow.SaveChangesAsync();
            return Result.Success(project);
        }
        return Result.Failure<Project>("The project is empty.");
    }
}
