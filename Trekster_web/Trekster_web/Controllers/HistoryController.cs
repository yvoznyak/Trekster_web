using Microsoft.AspNetCore.Mvc;
using Trekster_web.Filters;

namespace Trekster_web.Controllers
{
    [AuthorizeFilter]
    public class HistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
