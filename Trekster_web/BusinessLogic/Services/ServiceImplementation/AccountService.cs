using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services.ServiceImplementation
{
    public class AccountService : IAccountService
    {
        private readonly IAccount _account;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IAccount account,
            IMapper mapper,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AccountService> logger
        )
        {
            _account = account;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public IEnumerable<AccountModel> GetAll()
        {
            var user = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;
            _logger.LogInformation($"Get all Accounts");
            var entities = _account.GetAll().Where(x => x.UserId == user.Id.ToString());
            return _mapper.Map<List<AccountModel>>(entities);
        }

        public AccountModel GetById(int accountId)
        {
            var entity = _account.GetById(accountId);
            _logger.LogInformation($"Get account with id={accountId}.");
            return _mapper.Map<AccountModel>(entity);
        }

        public void Save(AccountModel account)
        {
            var user = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;
            account.UserId = user.Id.ToString();
            _account.Save(_mapper.Map<Account>(account));
            _logger.LogInformation($"Account saved with id={account.UserId}.");
        }

        public void Delete(int accountId)
        {
            _account.Delete(accountId);
        }

        public AccountModel GetLast()
        {
            var account = _account.GetAll().OrderByDescending(x => x.Id).FirstOrDefault();
            _logger.LogInformation($"Last account name ={account.Name}.");
            return _mapper.Map<AccountModel>(account);
        }
    }
}
