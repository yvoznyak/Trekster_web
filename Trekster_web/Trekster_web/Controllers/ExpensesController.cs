using BusinessLogic.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Trekster_web.ControllerServices.Interfaces;
using Trekster_web.Filters;
using Trekster_web.Models;

namespace Trekster_web.Controllers
{
    [AuthorizeFilter]
    public class ExpensesController : Controller
    {
        private readonly IExpensesControllerService _expenses;

        public ExpensesController(IExpensesControllerService expenses)
        {
            _expenses = expenses;
        }

        public IActionResult Index()
        {
            var expencesVM = new ExpencesVM();
            expencesVM.Summary = _expenses.GetSummary();
            expencesVM.ExpencesByCategory = _expenses.GetExpencesByCategory();
            return View(expencesVM);
        }
    }
}
