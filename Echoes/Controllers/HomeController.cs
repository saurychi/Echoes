using Echoes.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Echoes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            return View();
        }
    }
}
