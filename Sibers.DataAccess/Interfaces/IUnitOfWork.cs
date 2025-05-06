using Microsoft.AspNetCore.Identity;
using Sibers.Core.Entities;

namespace Sibers.DataAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    UserManager<User> UserManager { get; }
    IRepository<Project> ProjectManager { get; }
    SignInManager<User> SignInManager { get; }
    Task SaveChangesAsync();
}
