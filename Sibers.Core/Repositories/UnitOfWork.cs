using Microsoft.AspNetCore.Identity;
using Sibers.Core.Entities;
using Sibers.Core.Interfaces;

namespace Sibers.Core.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly UserManager<User> _userManager;
    private readonly IRepository<Project> _projectRepository;
    private readonly SibersContext _context;
    public UnitOfWork(SibersContext context ,UserManager<User> userManager, IRepository<Project> projectRepository)
    {
        _context = context;
        _userManager = userManager;
        _projectRepository = projectRepository;
    }
    public UserManager<User> UserManager => _userManager;

    public IRepository<Project> ProjectManager => _projectRepository;

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
