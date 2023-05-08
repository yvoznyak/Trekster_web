using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using EmailService;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<StartBalanceService> _logger;

        public StartBalanceService(
            IStartBalance startBalance,
            IMapper mapper,
            ILogger<StartBalanceService> logger)
        {
            _startBalance = startBalance;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<StartBalanceModel> GetAll()
        {
            var entities = _startBalance.GetAll();
            _logger.LogInformation($"Get all balances");
            return _mapper.Map<List<StartBalanceModel>>(entities);
        }

        public IEnumerable<StartBalanceModel> GetAllForAccount(AccountModel accountModel)
        {
            var entities = _startBalance.GetAll().Where(x => x.AccountId == accountModel.Id);
            _logger.LogInformation($"Get all for accountId= {accountModel.Id}");
            return _mapper.Map<List<StartBalanceModel>>(entities);
        }

        public StartBalanceModel GetById(int startBalanceId)
        {
            var entity = _startBalance.GetById(startBalanceId);
            _logger.LogInformation($"Get balance with id = {startBalanceId}");
            return _mapper.Map<StartBalanceModel>(entity);
        }

        public void Save(StartBalanceModel startBalance)
        {
            startBalance.Sum = Math.Round(startBalance.Sum, 5);
            _logger.LogInformation($"Save balance with id = {startBalance.Id}");
            _startBalance.Save(_mapper.Map<StartBalance>(startBalance));
        }

        public void Delete(int startBalanceId)
        {
            _startBalance.Delete(startBalanceId);
            _logger.LogInformation($"Delete balance with id = {startBalanceId}");
        }
    }
}
