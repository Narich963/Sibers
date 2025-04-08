using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sibers.Core;
using Sibers.Core.Entities;
using Sibers.Core.Interfaces;
using Sibers.Core.Repositories;
using Sibers.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SibersContext>(opts => opts.UseSqlite(connection, b => b.MigrationsAssembly("Sibers.MVC")))
                .AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<SibersContext>();

builder.Services.AddTransient(typeof(IRepository<Project>), typeof(ProjectRepository));
builder.Services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddTransient(typeof(UserService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
