using Microsoft.AspNetCore.Mvc;
using Sibers.Core.Entities;
using Sibers.Services.Services;

namespace Sibers.MVC.Controllers;

public class ProjectsController : Controller
{
    private readonly ProjectService _projectService;

    public ProjectsController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var projectsResult = await _projectService.GetAllAsync();
        if (projectsResult.IsSuccess)
            return View(projectsResult.Value);
        return BadRequest(projectsResult.Error);
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
}
