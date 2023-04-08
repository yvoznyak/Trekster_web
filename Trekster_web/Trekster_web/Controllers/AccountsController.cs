﻿using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trekster_web.Models;

namespace Trekster_web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrencyService _currencies;
        private readonly IAccountService _account;
        private readonly IStartBalanceService _startBalances;

        public AccountsController(ICurrencyService currencies,
                                  IAccountService account,
                                  IStartBalanceService startBalances, 
                                  UserManager<User> userManager)
        {
            _userManager = userManager;
            _currencies = currencies;
            _account = account;
            _startBalances = startBalances;
        }

        public IActionResult Index()
        {
            return View();
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
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).ToString();

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

            return View(accountsVM);
        }
    }
}