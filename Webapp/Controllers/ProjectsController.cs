using Microsoft.AspNetCore.Mvc;

namespace Webapp.Controllers;

public class ProjectsController : Controller
{
    [Route("admin/projects")]
    public IActionResult Index()
    {
        return View();
    }
}
