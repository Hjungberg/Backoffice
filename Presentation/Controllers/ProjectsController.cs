using Buisiness.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation.Controllers;

public class ProjectsController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;

    public async Task<IActionResult> Index()
    {
        var model = new ProjectsViewModel
        {
            Projects = await _projectService.GetAllAsync();
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> AddAsync(AddProjectViewModel model)
    {
        var addProjectFormData = model.MapTo<AddProjectFormData>();
        var result = await _projectService.CreateProjectAsync(addProjectFormData);
        return Json(new {});
    }
    [HttpPut]
    public IActionResult Update(UpdateProjectViewModel model)
    {
        return Json(new { });
    }
    [HttpDelete]§
    public IActionResult Delete(string id)
    {
        return Json(new { });
    }
}
