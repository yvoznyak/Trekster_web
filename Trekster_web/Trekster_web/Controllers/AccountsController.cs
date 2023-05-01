using System.Security.Claims;
using AutoMapper;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Trekster_web.ControllerServices.Interfaces;
using Trekster_web.Filters;
using Trekster_web.Models;

namespace Trekster_web.Controllers
{
    [AuthorizeFilter]
    public class AccountsController : Controller
    {
        private readonly ICurrencyService _currencies;
        private readonly IAccountControllerService _account;

        public AccountsController(ICurrencyService currencies,
                                  IAccountControllerService account)
        {
            _currencies = currencies;
            _account = account;
        }

        public async Task<IActionResult> Index()
        {
            var accountsVM = new AccountsVM();
            accountsVM.AccountsSummary = _account.GetAccountsInfo();

            return View(accountsVM);
        }

        public IActionResult Create()
        {
            var accountsVM = new AccountsVM();

            accountsVM.StartBalances = new Dictionary<string, float>();

            foreach (var tmp in _currencies.GetAll())
            {
                accountsVM.StartBalances.Add(tmp.Name, 0);
            }

            return View(accountsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountsVM accountsVM)
        {
            if (ModelState.IsValid && _account.StartBalancesNotEmpty(accountsVM))
            {
                _account.SaveAccount(accountsVM);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(accountsVM);
            }
        }
    }
}
