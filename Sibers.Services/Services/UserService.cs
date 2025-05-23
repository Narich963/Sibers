﻿using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sibers.Core.Entities;
using Sibers.DataAccess.Interfaces;
using Sibers.Services.DTO;
using Sibers.Services.Interfaces;

namespace Sibers.Services.Services;

/// <summary>
/// Service for Users logic
/// </summary>
public class UserService : IUserService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<List<UserDTO>>> GetAllAsync()
    {
        var users = await _uow.UserManager.Users.Include(p => p.Projects).ToListAsync();
        return Result.Success(_mapper.Map<List<UserDTO>>(users));
    }

    public async Task<Result<UserDTO>> GetAsync(int? id)
    {
        if (id != null)
        {
            var user = await _uow.UserManager.Users.Include(u => u.Projects).FirstOrDefaultAsync(u => u.Id == id);
            var userDTO = _mapper.Map<UserDTO>(user);

            if (user != null)
                return Result.Success(userDTO);
            return Result.Failure<UserDTO>($"The user with Id = {id} was not found.");
        }
        return Result.Failure<UserDTO>("User id is null.");
    }

    public async Task<Result<UserDTO>> CreateAsync(UserDTO userDto)
    {
        if (userDto != null)
        {
            var user = _mapper.Map<User>(userDto);

            var result = await _uow.UserManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
            {
                return Result.Success(userDto);
            }
            return Result.Failure<UserDTO>("Creating a new user was failed.");
        }
        return Result.Failure<UserDTO>("The user you want to create is null.");
    }

    public async Task<Result<UserDTO>> Login(UserDTO userDTO)
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
                {
                    return Result.Success(userDTO);
                }
            }
        }
        return Result.Failure<UserDTO>("Failed to login.");
    }

    public async Task<Result<UserDTO>> LoginApi(UserDTO userDto)
    {
        if (userDto != null)
        {
            var user = await _uow.UserManager.FindByEmailAsync(userDto.Email);
            if (user != null)
                return Result.Success(userDto);
        }
        return Result.Failure<UserDTO>("Failed to login");
    }

    public async Task<Result> Logout()
    {
        await _uow.SignInManager.SignOutAsync();
        return Result.Success();
    }

    public async Task<Result<UserDTO>> Edit(UserDTO userDto)
    {
        if (userDto != null)
        {
            var user = await _uow.UserManager.FindByEmailAsync(userDto.Email);

            if (user == null)
                return Result.Failure<UserDTO>("Failed to edit the user.");

            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.MiddleName = userDto.MiddleName;
            user.LastName = userDto.LastName;
            user.Avatar = userDto.Avatar;

            var result = await _uow.UserManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Result.Failure<UserDTO>("Failed to edit the user.");

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                var token = await _uow.UserManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _uow.UserManager.ResetPasswordAsync(user, token, userDto.Password);
                if (passwordResult.Succeeded)
                {
                    return Result.Success(userDto);
                }

            }
        }
        return Result.Failure<UserDTO>("Failed to edit the user.");
    }
}
