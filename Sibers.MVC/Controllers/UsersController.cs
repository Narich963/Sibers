using Microsoft.AspNetCore.Mvc;
using Sibers.MVC.ViewModels.Users;
using Sibers.Services.DTO;
using Sibers.Services.Services;

namespace Sibers.MVC.Controllers;

public class UsersController : Controller
{
    private readonly UserService _userService;
    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index() => View(await _userService.GetAllAsync());

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
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
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
}
