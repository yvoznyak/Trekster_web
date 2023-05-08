using AutoMapper;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trekster_web.Controllers;
using Trekster_web.ControllerServices.Interfaces;
using Trekster_web.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Trekster_web.ControllerServices.Implementation
{
    public class HomeControllerService : IHomeControllerService
    {
        private readonly ILogger<HomeControllerService> _logger;
        private readonly ITransactionService _transaction;
        private readonly IAccountService _account;
        private readonly ICurrencyService _currency;
        private readonly ICategoryService _category;
        private readonly IStartBalanceService _startBalance;
        private readonly IMapper _mapper;

        public HomeControllerService(
            ILogger<HomeControllerService> logger,
            ITransactionService transaction,
            IMapper mapper,
            IAccountService accountService,
            ICurrencyService currency,
            ICategoryService category,
            IStartBalanceService startBalance
        )
        {
            _logger = logger;
            _transaction = transaction;
            _mapper = mapper;
            _account = accountService;
            _currency = currency;
            _category = category;
            _startBalance = startBalance;
        }

        public SelectList GetListOfAccounts()
        {
            var accounts = _account.GetAll();
            var dict = new Dictionary<string, string>();
            _logger.LogInformation($"Get List Of Accounts");
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

        public void SaveTransaction(TransactionVM transactionVM)
        {
            var array = transactionVM.AccountsAndCurency.Split(' ');
            transactionVM.AccountId = Convert.ToInt32(array[1]);
            transactionVM.CurrencyId = Convert.ToInt32(array[0]);
            _logger.LogInformation($"Save transaction with id={transactionVM.Id}");
            _transaction.Save(_mapper.Map<TransactionModel>(transactionVM));
        }

        public string GetSummary()
        {
            var dict = new Dictionary<int, double>();
            var currencies = _currency.GetAll();
            foreach (var cur in currencies)
            {
                dict.Add(cur.Id, 0);
            }

            var userAccounts = _account.GetAll();

            foreach (var account in userAccounts)
            {
                var startBalances = _startBalance.GetAllForAccount(account);

                foreach (var startBalance in startBalances)
                {
                    dict[startBalance.CurrencyId] += startBalance.Sum;
                }
            }

            var transactions = _transaction.GetAllForUser();

            foreach (var transaction in transactions)
            {
                dict[transaction.CurrencyId] += _transaction.GetFinalSum(transaction.Id);
            }


            var res = $"Загалом: ";

            foreach (var elem in dict)
            {
                if (elem.Value != 0)
                {
                    var currencyName = _currency.GetById(elem.Key).Name;
                    res += $"{elem.Value} {currencyName}, ";
                }
            }

            res = res.Remove(res.Length - 2);
            _logger.LogInformation($"Summary={res}");
            return res;
        }

        public double GetExpencesPercentage()
        {
            var transactions = _transaction.GetAllForUser();
            var profitTransactions = transactions.Where(x => _category
                                                 .GetById(x.CategoryId).Type == 1)
                                                 .Select(x => x.Sum)
                                                 .Sum();

            var accounts = _account.GetAll();

            if (!accounts.Any())
            {
                return 0;
            }

            foreach (var account in accounts)
            {
                var startBalances = _startBalance.GetAllForAccount(account);

                foreach (var startBalance in startBalances)
                {
                    profitTransactions += startBalance.Sum;
                }
            }

            var expenceTransactions = transactions.Where(x => _category
                                                 .GetById(x.CategoryId).Type == -1)
                                                 .Select(x => x.Sum)
                                                 .Sum();
            _logger.LogInformation($"Percentage of expences={Math.Round(100 * expenceTransactions / profitTransactions, 2)}");
            return Math.Round(100 * expenceTransactions / profitTransactions, 2);
        }

        public bool ButtonExist()
        {
            if (_account.GetAll().Any())
            {
                _logger.LogInformation($"Button exists");
                return true;
            }
            else
            {
                _logger.LogInformation($"Button do not exists");
                return false;
            }

        }
    }
}
