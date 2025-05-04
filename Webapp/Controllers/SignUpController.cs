using Microsoft.AspNetCore.Mvc;
using Webapp.Models;

namespace Webapp.Controllers
{
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(SignUpViewModel model)
        {
            return View();
        }
        [HttpPost]
        public IActionResult ExternalSignup()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ExternalSignupCallback()
        {
            return View();
        }
    
}
}
