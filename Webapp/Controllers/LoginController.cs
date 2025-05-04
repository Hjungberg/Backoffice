using Microsoft.AspNetCore.Mvc;
using Webapp.Models;

namespace Webapp.Controllers;

public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Index(LoginViewModel model)
    {
        return View();
    }
}
