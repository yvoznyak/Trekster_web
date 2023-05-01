using AutoMapper;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Trekster_web.ControllerServices.Interfaces;
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
        private readonly IHistoryControllerService _history;

        public HistoryController(ITransactionService transaction,
                                 IAccountService account,
                                 ICategoryService category,
                                 ICurrencyService currency,
                                 IStartBalanceService startBalance,
                                 IMapper mapper,
                                 IHistoryControllerService history)
        {
            _transaction = transaction;
            _account = account;
            _category = category;
            _currency = currency;
            _mapper = mapper;
            _startBalance = startBalance;
            _history = history;
        }

        public IActionResult Index()
        {
            var historyVM = new HistoryVM();
            historyVM.Transactions = _transaction.GetAllForUser();
            historyVM.TransactionInfo = _history.GetTransactionInfo();
            return View(historyVM);
        }

        public async Task<IActionResult> EditTransaction(int id)
        {
            ViewData["AccountsAndCurency"] = _history.GetListOfAccounts();
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
                _history.SaveTransaction(transactionVM);
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
    }
}
