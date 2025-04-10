using Microsoft.AspNetCore.Mvc;
using Sibers.Core.Entities;
using Sibers.Core.Enums;
using Sibers.MVC.ViewModels.Projects;
using Sibers.Services.Services;

namespace Sibers.MVC.Controllers;

public class ProjectsController : Controller
{
    private readonly ProjectService _projectService;
    private readonly UserService _userService;
    private const int PAGE_SIZE = 2;

    public ProjectsController(ProjectService projectService, UserService userService)
    {
        _projectService = projectService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, string sortField = "StartDate", bool ascending = true,
        string? nameFilter = null,
        Priority? priorityFilter = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var total = await _projectService.Count();
        var projects = await _projectService.GetPagedAsync(page, sortField, ascending, nameFilter, priorityFilter, startDate, endDate, PAGE_SIZE);

        var model = new ProjectsListViewModel
        {
            Projects = projects,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(total / (double)PAGE_SIZE),
            SortField = sortField,
            Ascending = ascending,
            NameFilter = nameFilter,
            PriorityFilter = priorityFilter,
            StartDateFrom = startDate,
            StartDateTo = endDate
        };

        var users = await _userService.GetAllAsync();
        ViewBag.Users = users.Value;

         return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var projectResult = await _projectService.GetAsync(id);
        if (projectResult.IsSuccess)
            return View(projectResult.Value);
        return NotFound(projectResult.Error);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Project project)
    {
        if (ModelState.IsValid)
        {
            var projectResult = await _projectService.CreateAsync(project);
            if (projectResult.IsSuccess)
                return RedirectToAction("Details", new { id = projectResult.Value.Id });
            TempData["Error"] = projectResult.Error;
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (!id.HasValue)
            return NotFound();
        var projectResult = await _projectService.GetAsync(id.Value);
        if (projectResult.IsSuccess)
            return View(projectResult.Value);
        return BadRequest(projectResult.Error);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(Project newProject)
    {
        if (ModelState.IsValid)
        {
            var projectResult = await _projectService.Update(newProject);
            if (projectResult.IsSuccess)
                return RedirectToAction("Details", new { id = projectResult.Value.Id });
            return BadRequest(projectResult.Error);
        }
        return View(newProject);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id != null)
        {
            var result = await _projectService.Delete(id.Value);
            if (result.IsSuccess)
                return RedirectToAction("Index");
            return BadRequest(result.Error);
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> SetManager(int? userId, int? projectId)
    {
        var result = await _projectService.SetEmployee(userId, projectId, true);
        if (result.IsSuccess)
            return RedirectToAction("Index");
        return BadRequest();
    }
    public async Task<IActionResult> SetEmployee(int? userId, int? projectId)
    {
        var result = await _projectService.SetEmployee(userId, projectId, false);
        if (result.IsSuccess)
            return RedirectToAction("Index");
        return BadRequest();
    }
}
