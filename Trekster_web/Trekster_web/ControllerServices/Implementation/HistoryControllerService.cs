using AutoMapper;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Xml.Linq;
using Trekster_web.ControllerServices.Interfaces;
using Trekster_web.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Trekster_web.ControllerServices.Implementation
{
    public class HistoryControllerService : IHistoryControllerService
    {
        private readonly ITransactionService _transaction;
        private readonly IAccountService _account;
        private readonly ICategoryService _category;
        private readonly ICurrencyService _currency;
        private readonly IStartBalanceService _startBalance;
        private readonly IMapper _mapper;
        private readonly ILogger<HistoryControllerService> _logger;

        public HistoryControllerService(
            ITransactionService transaction,
            IAccountService account,
            ICategoryService category,
            ICurrencyService currency,
            IStartBalanceService startBalance,
            IMapper mapper,
            ILogger<HistoryControllerService> logger
        )
        {
            _transaction = transaction;
            _account = account;
            _category = category;
            _currency = currency;
            _mapper = mapper;
            _startBalance = startBalance;
            _logger = logger;
        }

        public Dictionary<int, Dictionary<string, string>> GetTransactionInfo()
        {
            var dict = new Dictionary<int, Dictionary<string, string>>();
            var lst = _transaction.GetAllForUser();
            lst.Reverse();

            foreach (var transaction in lst)
            {
                var text = $"{transaction.Date.ToString("dd.mm.yyyy")}," +
                           $" {_account.GetById(transaction.AccountId).Name}," +
                           $" {_category.GetById(transaction.CategoryId).Name}," +
                           $" {_category.GetById(transaction.CategoryId).Type * transaction.Sum}" +
                           $" {_currency.GetById(transaction.CurrencyId).Name}";

                var tmpDict = new Dictionary<string, string>();

                if (_category.GetById(transaction.CategoryId).Type == 1)
                {
                    tmpDict.Add(text, "#d4edda");
                }
                else
                {
                    tmpDict.Add(text, "#f8d7da");
                }

                _logger.LogInformation($"Get transactionInfo={text}");
                dict.Add(transaction.Id, tmpDict);
            }

            return dict;
        }

        public SelectList GetListOfAccounts()
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
                    _logger.LogInformation($"Get list of Accounts={text1}, {text2}");
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

        public void SaveTransaction(TransactionVM transactionVM)
        {
            var array = transactionVM.AccountsAndCurency.Split(' ');
            transactionVM.AccountId = Convert.ToInt32(array[1]);
            transactionVM.CurrencyId = Convert.ToInt32(array[0]);
            _logger.LogInformation($"Save transaction with id={transactionVM.Id}");
            _transaction.Save(_mapper.Map<TransactionModel>(transactionVM));
        }
    }
}
