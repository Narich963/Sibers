using Microsoft.AspNetCore.Identity;
using Sibers.Core.Entities;

namespace Sibers.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    UserManager<User> UserManager { get; }
    IRepository<Project> ProjectManager { get; }
    Task SaveChangesAsync();
}
