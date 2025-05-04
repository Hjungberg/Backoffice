using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class OverwiewController : Controller
{

    [Route("admin/overview")]
    public IActionResult Index()
    {
        return View();
    }
}
