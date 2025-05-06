using CSharpFunctionalExtensions;
using Sibers.Core.Entities;
using Sibers.Services.DTO;

namespace Sibers.Services.Interfaces;

public interface IUserService
{
    Task<Result<List<UserDTO>>> GetAllAsync();
    Task<Result<UserDTO>> GetAsync(int? id);
    Task<Result<UserDTO>> CreateAsync(UserDTO userDto);
    Task<Result<UserDTO>> Login(UserDTO userDto);
    Task<Result> Logout();
    Task<Result<UserDTO>> Edit(UserDTO userDto);
}
