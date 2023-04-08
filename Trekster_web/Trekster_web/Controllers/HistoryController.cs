using Microsoft.AspNetCore.Mvc;

namespace Trekster_web.Controllers
{
    public class HistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
