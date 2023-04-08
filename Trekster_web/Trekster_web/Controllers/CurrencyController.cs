using BusinessLogic.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Trekster_web.Filters;
using Trekster_web.Models;

namespace Trekster_web.Controllers
{
    [AuthorizeFilter]
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _currency;

        public CurrencyController(ICurrencyService currency)
        {
            _currency = currency;
        }

        public IActionResult Index()
        {
            var currencyVM = new CurrencyVM();
            currencyVM.CurrencyModels = _currency.GetAll();
            return View(currencyVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CurrencyModel")] CurrencyVM currencyVM)
        {
            _currency.Save(currencyVM.CurrencyModel);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var currencyVM = new CurrencyVM();
            currencyVM.CurrencyModel = _currency.GetById(id);
            return View(currencyVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CurrencyModel")] CurrencyVM currencyVM)
        {
            _currency.Save(currencyVM.CurrencyModel);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var currencyVM = new CurrencyVM();
            currencyVM.CurrencyModel = _currency.GetById(id);
            return View(currencyVM);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _currency.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
