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
    public class StartBalanceService : IStartBalanceService
    {
        protected readonly IStartBalance _startBalance;
        protected readonly IMapper _mapper;

        public StartBalanceService(IStartBalance startBalance, IMapper mapper)
        {
            _startBalance = startBalance;
            _mapper = mapper;
        }

        public IEnumerable<StartBalanceModel> GetAll()
        {
            var entities = _startBalance.GetAll();
            return _mapper.Map<List<StartBalanceModel>>(entities);
        }

        public IEnumerable<StartBalanceModel> GetAllForAccount(AccountModel accountModel)
        {
            var entities = _startBalance.GetAll().Where(x => x.AccountId == accountModel.Id);
            return _mapper.Map<List<StartBalanceModel>>(entities);
        }

        public StartBalanceModel GetById(int startBalanceId)
        {
            var entity = _startBalance.GetById(startBalanceId);
            return _mapper.Map<StartBalanceModel>(entity);
        }

        public void Save(StartBalanceModel startBalance)
        {
            startBalance.Sum = Math.Round(startBalance.Sum, 5);

            _startBalance.Save(_mapper.Map<StartBalance>(startBalance));
        }

        public void Delete(int startBalanceId)
        {
            _startBalance.Delete(startBalanceId);
        }
    }
}
