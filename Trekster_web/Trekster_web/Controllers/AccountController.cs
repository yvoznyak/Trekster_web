using BusinessLogic.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Trekster_web.Filters;
using Trekster_web.Models;

namespace Trekster_web.Controllers
{
    [AuthorizeFilter]
    public class AccountController : Controller
    {
        private readonly IAccountService _account;

        public AccountController(IAccountService account)
        {
            _account = account;
        }

        public async Task<IActionResult> Index()
        {
            var accountVM = new AccountVM();
            accountVM.Accounts = _account.GetAll();
            return View(accountVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Account")] AccountVM accountVM)
        {
            _account.Save(accountVM.Account);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var accountVM = new AccountVM();
            accountVM.Account = _account.GetById(id);
            return View(accountVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Account")] AccountVM accountVM)
        {
            _account.Save(accountVM.Account);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var accountVM = new AccountVM();
            accountVM.Account = _account.GetById(id);
            return View(accountVM);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _account.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
