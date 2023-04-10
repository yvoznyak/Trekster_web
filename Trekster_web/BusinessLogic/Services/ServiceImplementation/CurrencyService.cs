using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;

namespace BusinessLogic.Services.ServiceImplementation
{
    public class CurrencyService : ICurrencyService
    {
        protected readonly ICurrency _currency;
        protected readonly IStartBalanceService _startBalance;
        protected readonly IMapper _mapper;

        public CurrencyService(ICurrency currency, IMapper mapper, IStartBalanceService startBalance)
        {
            _currency = currency;
            _mapper = mapper;
            _startBalance = startBalance;
        }

        public IEnumerable<CurrencyModel> GetAll()
        {
            var entities = _currency.GetAll();
            return _mapper.Map<List<CurrencyModel>>(entities);
        }

        public IEnumerable<CurrencyModel> GetAllByAccount(AccountModel accountModel)
        {
            var startBalancesCurrencies = new List<Currency>();
            var startBalancesCurenciesId = _startBalance.GetAllForAccount(accountModel).Select(x => x.CurrencyId);

            foreach (var currencyId in startBalancesCurenciesId)
            {
                startBalancesCurrencies.Add(_currency.GetById(currencyId));
            }

            return _mapper.Map<List<CurrencyModel>>(startBalancesCurrencies);
        }

        public CurrencyModel GetById(int currencyId)
        {
            var entity = _currency.GetById(currencyId);
            return _mapper.Map<CurrencyModel>(entity);
        }

        public void Save(CurrencyModel currency)
        {
            _currency.Save(_mapper.Map<Currency>(currency));
        }

        public void Delete(int currencyId)
        {
            _currency.Delete(currencyId);
        }

        public CurrencyModel GetByName(string name)
        {
            var currency = _currency.GetAll().First(x => x.Name == name);
            return _mapper.Map<CurrencyModel>(currency);
        }
    }
}
