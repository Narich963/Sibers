using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sibers.MVC.ViewModels.Users;
using Sibers.Services.DTO;
using Sibers.Services.Interfaces;

namespace Sibers.MVC.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> logger)
    {
        _userService = userService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Page with all the users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _userService.GetAllAsync();
        var usersViewModel = _mapper.Map<List<UserViewModel>>(result.Value);
        return View(usersViewModel);
    }
        
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Create() => View(new CreateUserViewModel());

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userDto = _mapper.Map<UserDTO>(model);

            var userResult = await _userService.CreateAsync(userDto);
            if (userResult.IsSuccess)
                return RedirectToAction("Index");
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
            var userDto = _mapper.Map<UserDTO>(model);
            var loginResult = await _userService.Login(userDto);
            if (loginResult.IsSuccess)
            {
                _logger.LogInformation("User {User} logged in successfully", model.Email);
                return RedirectToAction("Index", "Projects");
            }
            _logger.LogError("User {User} failed to login", model.Email);
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
            var model = _mapper.Map<CreateUserViewModel>(userResult.Value);
            return View(model);
        }
        return NotFound(userResult.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userDto = _mapper.Map<UserDTO>(model);

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
        {
            var userViewModel = _mapper.Map<UserViewModel>(userResult.Value);
            return View(userViewModel);
        }
        return NotFound(userResult.Error);
    }
}
