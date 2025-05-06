using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sibers.Core;
using Sibers.Core.Entities;
using Sibers.Core.Interfaces;
using Sibers.Core.Repositories;
using Sibers.MVC.Initializers;
using Sibers.Services.Interfaces;
using Sibers.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SibersContext>(opts => opts.UseSqlite(connection, b => b.MigrationsAssembly("Sibers.MVC")))
                .AddIdentity<User, IdentityRole<int>>(opts =>
                {
                    opts.Password.RequiredLength = 5;
                    opts.Password.RequireDigit = false;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<SibersContext>()
                .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, AvatarUserClaimsPrincipalFactory>();

builder.Services.AddAutoMapper(typeof(AutoMapperInitializer));

builder.Services.AddTransient(typeof(IRepository<Project>), typeof(ProjectRepository));
builder.Services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddTransient(typeof(IUserService), typeof(UserService));
builder.Services.AddTransient(typeof(IProjectService), typeof(ProjectService));

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/Users/Login";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedUsersAsync(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Projects}/{action=Index}/{id?}")
    .WithStaticAssets();

Log.Information("The app is starting...");
app.Run();
Log.Information("The app started successfully.");
