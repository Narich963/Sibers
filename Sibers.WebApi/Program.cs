using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sibers.Core.Entities;
using Sibers.DataAccess;
using Sibers.DataAccess.Interfaces;
using Sibers.DataAccess.Repositories;
using Sibers.Services.Interfaces;
using Sibers.Services.Services;
using Sibers.WebApi.IdentityServer;
using Sibers.WebApi.Initializers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Sibers API", Version = "v1" });
    opts.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("https://localhost:5001/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    {"sibers_api", "Sibers Api" }
                }
            }
        }
    });

    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { 
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            ["sibers_api"]
        }
    });
});

var identityServerClient = builder.Configuration
    .GetSection("IdentityServer:Client")
    .Get<IdentityServerClient>();

var identityServerScope = builder.Configuration
    .GetSection("IdentityServer:ApiScope")
    .Get<IdentityServerScope>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.RequireHttpsMetadata = true;
        opts.SaveToken = true;
        opts.Authority = "https://localhost:5001";
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidIssuer = "https://localhost:5001",
        };

        opts.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services
    .AddDbContext<SibersContext>(opts => opts.UseSqlite(connection, m => m.MigrationsAssembly("Sibers.DataAccess")))
    .AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<SibersContext>();


builder.Services.AddIdentityServer()
    .AddInMemoryClients([new Client {
            ClientId = identityServerClient.ClientId,
            ClientSecrets = { new Secret(identityServerClient.ClientSecret.Sha256()) },
            AllowedGrantTypes = identityServerClient.AllowedGrantTypes,
            AllowedScopes = identityServerClient.AllowedScopes
        }])
    .AddInMemoryApiScopes([new ApiScope {
        Name = identityServerScope.Name,
        DisplayName = identityServerScope.DisplayName
    }])
    .AddAspNetIdentity<User>()
    .AddDeveloperSigningCredential();


builder.Services.AddAutoMapper(typeof(AutoMapperInitializer));
builder.Services.AddTransient<IRepository<Project>, ProjectRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "Sibers Web API");

        opts.OAuthClientId(identityServerClient.ClientId);
        opts.OAuthClientSecret(identityServerClient.ClientSecret);
        opts.OAuthAppName("Sibers API");
        opts.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();
