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

        builder.Entity<User>()
            .Property(x => x.Avatar)
            .HasMaxLength(500);
    }
}
