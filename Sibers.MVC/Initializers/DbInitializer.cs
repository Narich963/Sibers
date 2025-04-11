using Microsoft.AspNetCore.Identity;
using Sibers.Core;
using Sibers.Core.Entities;
using Sibers.Core.Enums;

namespace Sibers.MVC.Initializers;

public static class DbInitializer
{
    public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

        // Добавим пользователей
        var users = new List<(string Email, string Password, string FirstName, string LastName, string MiddleName)>
        {
            ("admin@example.com", "Admin123!", "Admin", "User", ""),
            ("john@example.com", "Password123!", "John", "Doe", ""),
            ("jane@example.com", "Password123!", "Jane", "Smith", "")
        };

        List<User> addedUsers = new List<User>();

        foreach (var (email, password, firstName, lastName, middleName) in users)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new User
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    MiddleName = middleName,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                    addedUsers.Add(user);
            }
        }

        await SeedProjectsAsync(serviceProvider, addedUsers);
    }

    public static async Task SeedProjectsAsync(IServiceProvider serviceProvider, List<User> users)
    {
        var dbContext = serviceProvider.GetRequiredService<SibersContext>();

        // Проверим, есть ли уже проекты в базе данных
        if (dbContext.Projects.Any()) return;

        // Добавим проекты
        var projects = new List<Project>
        {
            new Project
            {
                Name = "Project A",
                CustomerCompanyName = "Company A",
                ExecutorCompanyName = "Executor A",
                StartDate = DateTime.Now.AddMonths(-1),
                EndDate = DateTime.Now.AddMonths(3),
                Priority = Priority.High,
                ManagerUserId = users.FirstOrDefault()?.Id,
                Employees = new List<User> { users.FirstOrDefault() }
            },
            new Project
            {
                Name = "Project B",
                CustomerCompanyName = "Company B",
                ExecutorCompanyName = "Executor B",
                StartDate = DateTime.Now.AddMonths(-2),
                EndDate = DateTime.Now.AddMonths(4),
                Priority = Priority.Middle,
                ManagerUserId = users.LastOrDefault()?.Id,
                Employees = new List<User> { users.LastOrDefault() }
            }
        };

        dbContext.Projects.AddRange(projects);
        await dbContext.SaveChangesAsync();
    }
}
