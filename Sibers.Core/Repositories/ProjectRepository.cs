﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sibers.Core.Entities;
using Sibers.Core.Interfaces;

namespace Sibers.Core.Repositories;

public class ProjectRepository : IRepository<Project>
{
    private readonly SibersContext _context;
    public ProjectRepository(SibersContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects
                    .Include(p => p.Manager)
                    .Include(p => p.Employees)
                    .ToListAsync();
    }
    public IQueryable<Project> GetAllAsQueryable()
    {
        return _context.Projects
            .Include(p => p.Manager)
            .Include(p => p.Employees)
            .AsQueryable();
    }

    public async Task<IEnumerable<Project>> FindAsync(Expression<Func<Project, bool>> predicate)
    {
        return await _context.Projects.Where(predicate).ToListAsync();
    }

    public async Task<Project?> Get(int id) => await _context.Projects.Include(p => p.Manager).Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == id);

    public async Task Create(Project entity)
    {
        await _context.AddAsync(entity);
    }

    public void Update(Project entity)
    {
        if (entity != null)
            _context.Update(entity);
    }

    public void Delete(int id)
    {
        var project = _context.Projects.Find(id.ToString());
        if (project != null)
            _context.Projects.Remove(project);
    }
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<int> Count() => await _context.Projects.CountAsync();
    
}
