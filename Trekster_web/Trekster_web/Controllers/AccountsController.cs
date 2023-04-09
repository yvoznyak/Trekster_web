using System.Security.Claims;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Trekster_web.Filters;
using Trekster_web.Models;

namespace Trekster_web.Controllers
{
    [AuthorizeFilter]
    public class AccountsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrencyService _currencies;
        private readonly IAccountService _account;
        private readonly IStartBalanceService _startBalances;
        private readonly ITransactionService _transaction;

        public AccountsController(ICurrencyService currencies,
                                  IAccountService account,
                                  IStartBalanceService startBalances,
                                  ITransactionService transaction,
                                  UserManager<User> userManager)
        {
            _userManager = userManager;
            _currencies = currencies;
            _account = account;
            _startBalances = startBalances;
            _transaction = transaction;
        }

        public async Task<IActionResult> Index()
        {
            var accountsVM = new AccountsVM();
            accountsVM.AccountsSummary = new List<string>();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userAccounts = _account.GetAll().Where(x => x.User.Id == userId);

            var dict = new Dictionary<AccountModel, Dictionary<string, double>>();

            foreach(var account in userAccounts)
            {
                var startBalances = _startBalances.GetAll().Where(x => x.Account.Id == account.Id);
                var tmpDict = new Dictionary<string, double>();

                foreach (var startBalance in startBalances)
                {
                    tmpDict.Add(startBalance.Currency.Name, startBalance.Sum);
                }

                var transactions = _transaction.GetAll().Where(x => x.Account.Id == account.Id);
                if (transactions.Any())
                {
                    foreach (var transaction in transactions)
                    {
                        tmpDict[transaction.Currency.Name] += _transaction.GetFinalSum(transaction.Id);
                    }
                }

                var text = $"{account.Name}: ";
                foreach (var key in tmpDict.Keys)
                {
                    text += $"{tmpDict[key]} {key} ";
                }

                accountsVM.AccountsSummary.Add(text);
            }

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
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var user = await _userManager.FindByIdAsync(userId);

                bool create = false;
                foreach (var tmp in accountsVM.StartBalances)
                {
                    if (tmp.Value != 0)
                    {
                        create = true;
                        break;
                    }
                }

                if (create)
                {
                    var accountModel = new AccountModel
                    {
                        Name = accountsVM.Name,
                        User = user
                    };

                    _account.Save(accountModel);

                    foreach (var tmp in accountsVM.StartBalances)
                    {
                        if (tmp.Value != 0)
                        {
                            var startBalanceModel = new StartBalanceModel
                            {
                                Sum = tmp.Value,
                                Account = _account.GetLast(),
                                Currency = _currencies.GetByName(tmp.Key),
                            };

                            _startBalances.Save(startBalanceModel);
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(accountsVM);
            }
        }
    }
}
