using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sibers.Services.Interfaces;
using Sibers.WebApi.ViewModels;

namespace Sibers.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }
    [HttpGet]
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
