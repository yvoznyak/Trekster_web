using Microsoft.AspNetCore.Mvc;

namespace Trekster_web.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Accounts()
        {
            return View();
        }

        public IActionResult Categories()
        {
            return View();
        }
    }
}
