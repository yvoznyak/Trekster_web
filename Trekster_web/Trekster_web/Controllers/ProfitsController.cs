using Microsoft.AspNetCore.Mvc;
using Trekster_web.ControllerServices.Interfaces;
using Trekster_web.Filters;
using Trekster_web.Models;

namespace Trekster_web.Controllers
{
    [AuthorizeFilter]
    public class ProfitsController : Controller
    {
        private readonly IProfitsControllerService _profits;

        public ProfitsController(IProfitsControllerService profits)
        {
            _profits = profits;
        }

        public IActionResult Index()
        {
            var profitVM = new ProfitsVM();
            profitVM.Summary = _profits.GetSummary();
            profitVM.ProfitsByCategory = _profits.GetProfitsByCategory();
            return View(profitVM);
        }
    }
}
