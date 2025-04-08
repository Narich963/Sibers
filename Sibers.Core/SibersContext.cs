using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sibers.Core.Entities;
using System.Reflection.Emit;

namespace Sibers.Core;

public class SibersContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public SibersContext(DbContextOptions opts) : base(opts)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Project>()
            .HasMany(e => e.Employees)
            .WithMany(p => p.Projects)
            .UsingEntity<Dictionary<string, object>>(
                "ProjectEmployees",
                j => j.HasOne<User>().WithMany().HasForeignKey("EmployeesId"),
                j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectsId"))
            .HasKey("EmployeesId", "ProjectsId");

        builder.Entity<Project>()
            .HasOne(p => p.Manager)
            .WithMany()
            .HasForeignKey(p => p.ManagerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Project>().HasData(
            new Project
            {
                Id = 1,
                Name = "CRM Development",
                CustomerCompanyName = "TechCorp",
                ExecutorCompanyName = "DevSolutions",
                StartDate = null,
                EndDate = null,
                Priority = 1,
                ManagerUserId = null
            },
            new Project
            {
                Id = 2,
                Name = "Website Redesign",
                CustomerCompanyName = "DesignStudio",
                ExecutorCompanyName = "UIExperts",
                StartDate = null,
                EndDate = null,
                Priority = 2,
                ManagerUserId = null
            }
        );

        builder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int>
            {
                Id = 1,
                Name = "leader",
                NormalizedName = "LEADER"
            },
            new IdentityRole<int>
            {
                Id = 2,
                Name = "manager",
                NormalizedName = "MANAGER"
            },
            new IdentityRole<int>
            {
                Id = 3,
                Name = "employee",
                NormalizedName = "EMPLOYEE"
            }
        );
    }
}
