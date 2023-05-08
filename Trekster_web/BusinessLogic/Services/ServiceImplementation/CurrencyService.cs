using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services.ServiceImplementation
{
    public class CurrencyService : ICurrencyService
    {
        protected readonly ICurrency _currency;
        protected readonly IStartBalanceService _startBalance;
        protected readonly IMapper _mapper;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(
            ICurrency currency,
            IMapper mapper,
            IStartBalanceService startBalance,
            ILogger<CurrencyService> logger
        )
        {
            _currency = currency;
            _mapper = mapper;
            _startBalance = startBalance;
            _logger = logger;
        }

        public IEnumerable<CurrencyModel> GetAll()
        {
            var entities = _currency.GetAll();
            _logger.LogInformation($"Get all models");
            return _mapper.Map<List<CurrencyModel>>(entities);
        }

        public IEnumerable<CurrencyModel> GetAllByAccount(AccountModel accountModel)
        {
            var startBalancesCurrencies = new List<Currency>();
            var startBalancesCurenciesId = _startBalance.GetAllForAccount(accountModel).Select(x => x.CurrencyId);

            foreach (var currencyId in startBalancesCurenciesId)
            {
                _logger.LogInformation($"Add currency with id= {currencyId}");
                startBalancesCurrencies.Add(_currency.GetById(currencyId));
            }

            return _mapper.Map<List<CurrencyModel>>(startBalancesCurrencies);
        }

        public CurrencyModel GetById(int currencyId)
        {
            var entity = _currency.GetById(currencyId);
            _logger.LogInformation($"Get currency with id= {currencyId}");
            return _mapper.Map<CurrencyModel>(entity);
        }

        public void Save(CurrencyModel currency)
        {
            _logger.LogInformation($"Get currency with id= {currency.Id}");
            _currency.Save(_mapper.Map<Currency>(currency));
        }

        public void Delete(int currencyId)
        {
            _currency.Delete(currencyId);
            _logger.LogInformation($"Delete currency with id= {currencyId}");
        }

        public CurrencyModel GetByName(string name)
        {
            var currency = _currency.GetAll().First(x => x.Name == name);
            _logger.LogInformation($"Get currency by name={name}");
            return _mapper.Map<CurrencyModel>(currency);
        }
    }
}
