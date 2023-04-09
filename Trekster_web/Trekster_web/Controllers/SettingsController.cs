using Microsoft.AspNetCore.Mvc;
using Trekster_web.Filters;

namespace Trekster_web.Controllers
{
    [AuthorizeFilter]
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
