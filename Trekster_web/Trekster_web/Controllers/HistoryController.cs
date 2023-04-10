using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
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

        public HistoryController(ITransactionService transaction,
                                 IAccountService account,
                                 ICategoryService category,
                                 ICurrencyService currency)
        {
            _transaction = transaction;
            _account = account;
            _category = category;
            _currency = currency;
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
            /*var accountVM = new AccountVM();
            accountVM = _mapper.Map<AccountVM>(_account.GetById(id));*/
            return View(/*accountVM*/);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTransaction(int id, AccountVM accountVM)
        {
            /*if (ModelState.IsValid)
            {
                _account.Save(_mapper.Map<AccountModel>(accountVM));
                return RedirectToAction(nameof(Accounts));
            }*/

            return View(accountVM);
        }

        public async Task<IActionResult> DeleteTransaction(int id)
        {
            return View(/*_mapper.Map<AccountVM>(_account.GetById(id))*/);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTransaction(int id, AccountVM accountVM)
        {
            /*if (ModelState.IsValid)
            {
                _account.Delete(id);
                return RedirectToAction(nameof(Accounts));
            }*/

            return View(accountVM);
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
    }
}
