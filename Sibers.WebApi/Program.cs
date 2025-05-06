using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sibers.Core.Entities;
using Sibers.DataAccess;
using Sibers.DataAccess.Interfaces;
using Sibers.DataAccess.Repositories;
using Sibers.Services.Interfaces;
using Sibers.Services.Services;
using Sibers.WebApi.Initializers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services
    .AddDbContext<SibersContext>(opts => opts.UseSqlite(connection, m => m.MigrationsAssembly("Sibers.DataAccess")))
    .AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<SibersContext>();

builder.Services.AddAutoMapper(typeof(AutoMapperInitializer));
builder.Services.AddTransient<IRepository<Project>, ProjectRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
