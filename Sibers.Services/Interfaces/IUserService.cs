using CSharpFunctionalExtensions;
using Sibers.Core.Entities;
using Sibers.Services.DTO;

namespace Sibers.Services.Interfaces;

public interface IUserService
{
    Task<Result<List<User>>> GetAllAsync();
    Task<Result<User>> GetAsync(int? id);
    Task<Result<User>> CreateAsync(UserDTO userDto);
    Task<Result<User>> Login(UserDTO userDto);
    Task<Result> Logout();
    Task<Result<User>> Edit(UserDTO userDto);
}
