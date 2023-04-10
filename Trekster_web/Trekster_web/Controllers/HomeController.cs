using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trekster_web.Filters;
using Trekster_web.Models;

namespace Trekster_web.Controllers
{
    [AuthorizeFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITransactionService _transaction;
        private readonly IAccountService _account;
        private readonly ICurrencyService _currency;
        private readonly ICategoryService _category;
        private readonly IStartBalanceService _startBalance;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger,
                              ITransactionService transaction,
                              IMapper mapper,
                              IAccountService accountService,
                              ICurrencyService currency,
                              ICategoryService category,
                              IStartBalanceService startBalance)
        {
            _logger = logger;
            _transaction = transaction;
            _mapper = mapper;
            _account = accountService;
            _currency = currency;
            _category = category;
            _startBalance = startBalance;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateTransaction()
        {
            ViewData["AccountsAndCurency"] = GetListOfAccounts();
            ViewData["Categories"] = new SelectList(_category.GetAll(), "Id", "Name");
            var transactionVM = new TransactionVM();
            return View(transactionVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTransaction(TransactionVM transactionVM)
        {
            if (ModelState.IsValid)
            {
                _transaction.Save(_mapper.Map<TransactionModel>(GetTransactionVM(transactionVM)));
                return RedirectToAction(nameof(Index));
            }

            return View(transactionVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private SelectList GetListOfAccounts()
        {
            var accounts = _account.GetAll();
            var dict = new Dictionary<string, string>();

            foreach (var account in accounts)
            {
                var startBalances = _startBalance.GetAllForAccount(account);

                foreach (var startBalance in startBalances)
                {
                    var text1 = $"{account.Name}, {_currency.GetById(startBalance.CurrencyId).Name}";
                    var text2 = $"{startBalance.CurrencyId} {startBalance.AccountId}";
                    dict.Add(text1, text2);
                }
            }

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var entry in dict)
            {
                items.Add(new SelectListItem { Text = entry.Key, Value = entry.Value });
            }

            var selectList = new SelectList(items, "Value", "Text");

            return selectList;
        }

        private TransactionVM GetTransactionVM(TransactionVM transactionVM)
        {
            var array = transactionVM.AccountsAndCurency.Split(' ');
            transactionVM.AccountId = Convert.ToInt32(array[1]);
            transactionVM.CurrencyId = Convert.ToInt32(array[0]);

            return transactionVM;
        }
    }
}