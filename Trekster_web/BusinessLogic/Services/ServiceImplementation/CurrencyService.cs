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
        protected readonly IMapper _mapper;

        public CurrencyService(ICurrency currency, IMapper mapper)
        {
            _currency = currency;
            _mapper = mapper;
        }

        public IEnumerable<CurrencyModel> GetAll()
        {
            var entities = _currency.GetAll();
            return _mapper.Map<List<CurrencyModel>>(entities);
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

        public Currency GetByName(string name)
        {
            return _currency.GetAll().First(x => x.Name == name);
        }
    }
}
