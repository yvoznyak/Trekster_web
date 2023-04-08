using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
