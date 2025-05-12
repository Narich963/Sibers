using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sibers.Services.DTO;
using Sibers.Services.Interfaces;
using Sibers.WebApi.Contracts;
using Sibers.WebApi.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sibers.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public UsersController(IUserService userService, IMapper mapper, IConfiguration config)
    {
        _userService = userService;
        _mapper = mapper;
        _config = config;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("All")]
    public async Task<IActionResult> GetAllAsync()
    {
        var usersResult = await _userService.GetAllAsync();
        if (usersResult.IsSuccess)
        {
            var usersViewModel = _mapper.Map<List<UserResponse>>(usersResult.Value);
            return Ok(usersViewModel);
        }
        return BadRequest(usersResult.Error);
    }
}
