using AutoMapper;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trekster_web.Filters;
using Trekster_web.Models;

namespace Trekster_web.Controllers
{
    [AuthorizeFilter]
    public class HistoryController : Controller
    {
        private readonly ITransactionService _transaction;
        private readonly IAccountService _account;
        private readonly ICategoryService _category;
        private readonly ICurrencyService _currency;
        private readonly IStartBalanceService _startBalance;
        private readonly IMapper _mapper;

        public HistoryController(ITransactionService transaction,
                                 IAccountService account,
                                 ICategoryService category,
                                 ICurrencyService currency,
                                 IStartBalanceService startBalance,
                                 IMapper mapper)
        {
            _transaction = transaction;
            _account = account;
            _category = category;
            _currency = currency;
            _mapper = mapper;
            _startBalance = startBalance;
        }

        public IActionResult Index()
        {
            var historyVM = new HistoryVM();
            historyVM.Transactions = _transaction.GetAllForUser();
            historyVM.TransactionInfo = GetTransactionInfo();
            return View(historyVM);
        }

        public async Task<IActionResult> EditTransaction(int id)
        {
            ViewData["AccountsAndCurency"] = GetListOfAccounts();
            ViewData["Categories"] = new SelectList(_category.GetAll(), "Id", "Name");

            var transactionVM = _mapper.Map<TransactionVM>(_transaction.GetById(id));
            return View(transactionVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTransaction(int id, TransactionVM transactionVM)
        {
            if (ModelState.IsValid)
            {
                _transaction.Save(_mapper.Map<TransactionModel>(GetTransactionVM(transactionVM)));
                return RedirectToAction(nameof(Index));
            }

            return View(transactionVM);
        }

        public async Task<IActionResult> DeleteTransaction(int id)
        {
            return View(_mapper.Map<TransactionVM>(_transaction.GetById(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTransaction(int id, TransactionVM transactionVM)
        {
            if (ModelState.IsValid)
            {
                _transaction.Delete(id);
                return RedirectToAction(nameof(Index));
            }

            return View(transactionVM);
        }

        private Dictionary<int, string> GetTransactionInfo()
        {
            var dict = new Dictionary<int, string>();

            foreach (var transaction in _transaction.GetAll())
            {
                var text = $"{transaction.Date.Date}," +
                           $" {_account.GetById(transaction.AccountId).Name}" +
                           $" {_category.GetById(transaction.CategoryId).Name}" +
                           $" {_category.GetById(transaction.CategoryId).Type * transaction.Sum}" +
                           $" {_currency.GetById(transaction.CurrencyId).Name}";
                dict.Add(transaction.Id, text);
            }

            return dict;
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
