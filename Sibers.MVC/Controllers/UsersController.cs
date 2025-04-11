using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sibers.MVC.ViewModels.Users;
using Sibers.Services.DTO;
using Sibers.Services.Services;

namespace Sibers.MVC.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly UserService _userService;
    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _userService.GetAllAsync();
        return View(result.Value);
    }
        

    [HttpGet]
    public IActionResult Create() => View(new CreateUserViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userDto = new UserDTO
            {
                Email = model.Email,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Password = model.Password
            };

            var userResult = await _userService.CreateAsync(userDto);
            if (userResult.IsSuccess)
                return RedirectToAction("Details", new { userId = userResult.Value.Id });
            return BadRequest(userResult.Error);
        }
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userDto = new UserDTO
            {
                Email = model.Email,
                Password = model.Password
            };
            var loginResult = await _userService.Login(userDto);
            if (loginResult.IsSuccess)
                return RedirectToAction("Index", "Projects");
            return BadRequest();
        }
        return View(model);
    }
    public async Task<IActionResult> Logout()
    {
        var result = await _userService.Logout();
        if (result.IsSuccess)
            return RedirectToAction("Login", "Users");
        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        var userResult = await _userService.GetAsync(id);
        if (userResult.IsSuccess)
        {
            var model = new CreateUserViewModel
            {
                Email = userResult.Value?.Email,
                FirstName = userResult.Value?.FirstName,
                MiddleName = userResult.Value?.MiddleName,
                LastName = userResult.Value?.LastName,
            };
            return View(model);
        }
        return NotFound(userResult.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userDto = new UserDTO
            {
                Email = model.Email,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Password = model.Password
            };

            var result = await _userService.Edit(userDto);

            if (result.IsSuccess)
                return RedirectToAction("Index");
            return BadRequest();
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        var userResult = await _userService.GetAsync(id);
        if (userResult.IsSuccess)
            return View(userResult.Value);
        return NotFound(userResult.Error);
    }
}
