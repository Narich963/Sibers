using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Sibers.Core.Entities;
using Sibers.Core.Enums;
using Sibers.Core.Interfaces;
using System.Linq.Expressions;

namespace Sibers.Services.Services;

public class ProjectService
{
    private readonly IUnitOfWork _uow;
    public ProjectService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<IEnumerable<Project>>> GetAllAsync() => Result.Success(await _uow.ProjectManager.GetAllAsync());

    public async Task<IEnumerable<Project>> GetPagedAsync(
        int page,
        string sortField = "StartDate",
        bool ascending = true,
        string? name = null,
        Priority? priority = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int pageSize = 1)
    {
        var query = _uow.ProjectManager.GetAllAsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p => p.Name.Contains(name));
        }

        if (priority.HasValue)
        {
            query = query.Where(p => p.Priority == priority.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(p => p.StartDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(p => p.StartDate <= endDate.Value);
        }

        Expression<Func<Project, object>> selectorKey = sortField switch
        {
            "StartDate" => project => project.StartDate,
            "EndDate" => project => project.EndDate,
            "Priority" => project => (int)project.Priority,
            "Name" => project => project.Name,
            _ => project => project.Id
        };

        if (ascending)
            query = query.OrderBy(selectorKey);
        else
            query = query.OrderByDescending(selectorKey);

        return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

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

    public async Task<Result<Project>> Update(Project project)
    {
        if (project != null)
        {
            _uow.ProjectManager.Update(project);
            await _uow.SaveChangesAsync();
            return Result.Success(project);
        }
        return Result.Failure<Project>($"An error occured while trying to update project with Id = {project.Id}.");
    }

    public async Task<Result> Delete(int? id)
    {
        if (id != null)
        {
            _uow.ProjectManager.Delete(id.Value);
            await _uow.SaveChangesAsync();
            return Result.Success();
        }
        return Result.Failure($"An error ha occured while trying to delete a project.");
    }
    public async Task<int> Count() => await _uow.ProjectManager.Count();
}
