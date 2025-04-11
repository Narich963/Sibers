﻿using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Sibers.Core.Entities;
using Sibers.Core.Enums;
using Sibers.Core.Interfaces;
using System.Linq.Expressions;

namespace Sibers.Services.Services;

/// <summary>
/// Service for working with projects
/// </summary>
public class ProjectService
{
    private readonly IUnitOfWork _uow;
    public ProjectService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<IEnumerable<Project>>> GetAllAsync() => Result.Success(await _uow.ProjectManager.GetAllAsync());

    /// <summary>
    /// Get all sorted projects with filter
    /// </summary>
    /// <param name="page">Current page</param>
    /// <param name="sortField">Field that all projects are ordered by</param>
    /// <param name="ascending">Is ascending or descenging order</param>
    /// <param name="name">Project name</param>
    /// <param name="priority">Project priority</param>
    /// <param name="startDate">Date from</param>
    /// <param name="endDate">Date to</param>
    /// <param name="pageSize">How man items are in this page</param>
    /// <returns>Sorted projects with filter</returns>
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

    /// <summary>
    /// Set employee to the project
    /// </summary>
    /// <param name="userId">Employee Id</param>
    /// <param name="projectId">Project Id</param>
    /// <param name="isManager">Should set as employee or as manager</param>
    /// <returns></returns>
    public async Task<Result> SetEmployee(int? userId, int? projectId, bool isManager)
    {
        if (userId == null)
            return Result.Failure("User id is null");
        if (projectId == null)
            return Result.Failure("Project id is null");

        var user = await _uow.UserManager.FindByIdAsync(userId.Value.ToString());
        if (user != null)
        {
            var project = await _uow.ProjectManager.Get(projectId.Value);
            if (project == null)
                return Result.Failure($"No project with Id = {projectId} was found.");

            if (isManager)
                project.ManagerUserId = userId;
            else
                if (!project.Employees.Any(u => u.Id == user.Id))
                    project.Employees.Add(user);
                else
                    return Result.Failure($"This project already has Employee {user.FirstName}");


            await _uow.SaveChangesAsync();
            return Result.Success();
        }
        return Result.Failure("Failed to set a new manager to this project.");
    }

    /// <summary>
    /// Remove an employee from the project
    /// </summary>
    /// <param name="userId">Employee Id</param>
    /// <param name="projectId">ProjectId</param>
    /// <returns></returns>
    public async Task<Result> RemoveEmployee(int? userId, int? projectId)
    {
        if (!userId.HasValue || !projectId.HasValue)
            return Result.Failure("");

        var user = await _uow.UserManager.FindByIdAsync(userId.Value.ToString());
        var project = await _uow.ProjectManager.Get(projectId.Value);
        if (user != null && project != null)
        {
            project.Employees.Remove(user);
            await _uow.SaveChangesAsync();
            return Result.Success();
        }
        return Result.Failure($"Failed to remove {user.FirstName} from {project.Name}");
    }

}
