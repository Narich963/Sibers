using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sibers.Core.Entities;
using Sibers.Core.Interfaces;

namespace Sibers.Core.Repositories;

/// <summary>
/// The repository for interaction with Project entities
/// </summary>
public class ProjectRepository : IRepository<Project>
{
    private readonly SibersContext _context;
    public ProjectRepository(SibersContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects
                    .Include(p => p.Manager)
                    .Include(p => p.Employees)
                    .ToListAsync();
    }

    /// <inheritdoc/>
    public IQueryable<Project> GetAllAsQueryable()
    {
        return _context.Projects
            .Include(p => p.Manager)
            .Include(p => p.Employees)
            .AsQueryable();
    }

    /// <inheritdoc/>
    public async Task<Project?> Get(int id) => await _context.Projects.Include(p => p.Manager).Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == id);

    /// <inheritdoc/>
    public async Task Create(Project entity)
    {
        await _context.AddAsync(entity);
    }

    /// <inheritdoc/>
    public void Update(Project entity)
    {
        if (entity != null)
            _context.Update(entity);
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        var project = _context.Projects.Find(id);
        if (project != null)
            _context.Projects.Remove(project);
    }

    /// <inheritdoc/>
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<int> Count() => await _context.Projects.CountAsync();
}
