using Microsoft.AspNetCore.Mvc;

namespace Trekster_web.Controllers
{
    public class ExpensesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
