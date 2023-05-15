using AutoMapper;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceImplementation;
using BusinessLogic.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Trekster_web.ControllerServices.Interfaces;
using Trekster_web.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Trekster_web.ControllerServices.Implementation
{
    public class AccountControllerService : IAccountControllerService
    {
        private readonly ICurrencyService _currencies;
        private readonly IAccountService _account;
        private readonly IStartBalanceService _startBalances;
        private readonly ITransactionService _transaction;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountControllerService> _logger;

        public AccountControllerService(
            ICurrencyService currencies,
            IAccountService account,
            IStartBalanceService startBalances,
            ITransactionService transaction,
            IMapper mapper,
            ILogger<AccountControllerService> logger
        )
        {
            _currencies = currencies;
            _account = account;
            _startBalances = startBalances;
            _transaction = transaction;
            _mapper = mapper;
            _logger = logger;
        }

        public List<string> GetAccountsInfo()
        {
            var result = new List<string>();
            var userAccounts = _account.GetAll();

            foreach (var account in userAccounts)
            {
                var startBalances = _startBalances.GetAllForAccount(account);
                var tmpDict = new Dictionary<string, double>();

                foreach (var startBalance in startBalances)
                {
                    tmpDict.Add(_currencies.GetById(startBalance.CurrencyId).Name, startBalance.Sum);
                }

                var transactions = _transaction.GetAllForAccount(account);
                if (transactions.Any())
                {
                    foreach (var transaction in transactions)
                    {
                        tmpDict[_currencies.GetById(transaction.CurrencyId).Name] += _transaction.GetFinalSum(transaction.Id);
                    }
                }

                var text = $"{account.Name}: ";
                foreach (var key in tmpDict.Keys)
                {
                    text += $"{tmpDict[key]} {key} ";
                }
                _logger.LogInformation($"Get account info={text}");
                result.Add(text);
            }

            return result;
        }

        public bool StartBalancesNotEmpty(AccountsVM accountsVM)
        {
            bool create = false;
            foreach (var tmp in accountsVM.StartBalances)
            {
                if (tmp.Value != 0)
                {
                    create = true;
                    break;
                }
            }
            _logger.LogInformation($"Start balance is not empty={create}");
            return create;
        }

        public void SaveAccount(AccountsVM accountsVM)
        {
            var accountModel = new AccountModel();
            accountModel = _mapper.Map<AccountModel>(accountsVM);

            _account.Save(accountModel);

            foreach (var tmp in accountsVM.StartBalances)
            {
                if (tmp.Value != 0)
                {
                    var startBalanceModel = new StartBalanceModel
                    {
                        Sum = tmp.Value,
                        AccountId = _account.GetLast().Id,
                        CurrencyId = _currencies.GetByName(tmp.Key).Id,
                    };
                    _logger.LogInformation($"Save start balance with name={accountsVM.Name}");
                    _startBalances.Save(startBalanceModel);
                }
            }
        }
    }
}
