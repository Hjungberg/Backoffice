﻿using Buisiness.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Buisiness.Services;

public interface IUserService
{
    Task<UserResult> AddUserToRole(string userId, string roleName);
    Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User");
    Task<UserResult> GetUsersAsync();
}

public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<UserResult> GetUsersAsync()
    {

        var result = await _userRepository.GetAllAsync();
        return result.MapTo<UserResult>();
    }

    public async Task<UserResult> AddUserToRole(string userId, string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            return new UserResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "Role does not exist"
            };
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new UserResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "User not found"
            };
        }
        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded
            ? new UserResult{Succeeded = true, StatusCode = 200,}
            : new UserResult{Succeeded = false, StatusCode = 500, Error = "Unable to add user"};
    }
    public async Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User")
    {
        if (formData == null)
        {
            return new UserResult{Succeeded = false,StatusCode = 400,Error = "Form data is null"};
        }
        var existsResult = await _userRepository.ExistsAsync(x => x.Email == formData.Email);

        if (existsResult.Succeeded)
        {
            return new UserResult{Succeeded = false,StatusCode = 409,Error = "User already exists"};
        }
        try
        {
            var userEntity = formData.MapTo<UserEntity>();
            var result = await _userManager.CreateAsync(userEntity, formData.Password);

            if (result.Succeeded)
            {
                var addToRoleResult = await AddUserToRole(userEntity.Id, roleName);
                return result.Succeeded
                    ? new UserResult { Succeeded = true, StatusCode = 201 }
                    : new UserResult { Succeeded = false, StatusCode = 201, Error = "User created  but not added to role" };
            }
            return new UserResult { Succeeded = false, StatusCode = 500, Error = "Unable to add user" };

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new UserResult{Succeeded = false,StatusCode = 500,Error = ex.Message};
        }
    }

}
