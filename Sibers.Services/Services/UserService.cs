using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sibers.Core.Entities;
using Sibers.Core.Interfaces;
using Sibers.Services.DTO;

namespace Sibers.Services.Services;

public class UserService
{
    private readonly IUnitOfWork _uow;
    public UserService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<List<User>>> GetAllAsync() => Result.Success(await _uow.UserManager.Users.ToListAsync());

    public async Task<Result<User>> GetAsync(int? id)
    {
        if (id != null)
        {
            var user = await _uow.UserManager.FindByIdAsync(id.Value.ToString());
            if (user != null)
                return Result.Success(user);
            return Result.Failure<User>($"The user with Id = {id} was not found.");
        }
        return Result.Failure<User>("User id is null.");
    }

    public async Task<Result<User>> CreateAsync(UserDTO userDto)
    {
        if (userDto != null)
        {
            var user = new User
            {
                UserName = userDto.Email,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                MiddleName = userDto.MiddleName,
                LastName = userDto.LastName
            };

            var result = await _uow.UserManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
                return Result.Success(user);
            return Result.Failure<User>("Creating a new user was failed.");
        }
        return Result.Failure<User>("The user you want to create is null.");
    }

    public async Task<Result<User>> Login(UserDTO userDTO)
    {
        if (userDTO != null)
        {
            var user = await _uow.UserManager.FindByEmailAsync(userDTO.Email);
            if (user != null)
            {
                SignInResult result = await _uow.SignInManager.PasswordSignInAsync(
                    user,
                    userDTO.Password,
                    isPersistent: false,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                    return Result.Success(user);
            }
        }
        return Result.Failure<User>("Failed to login.");
    }
}
