using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace BusinessLogic.Services.ServiceImplementation
{
    public class AccountService : IAccountService
    {
        private readonly IAccount _account;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(IAccount account,
                              IMapper mapper,
                              UserManager<User> userManager,
                              IHttpContextAccessor httpContextAccessor)
        {
            _account = account;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<AccountModel> GetAll()
        {
            var user = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;

            var entities = _account.GetAll().Where(x => x.UserId == user.Id.ToString());
            return _mapper.Map<List<AccountModel>>(entities);
        }

        public AccountModel GetById(int accountId)
        {
            var entity = _account.GetById(accountId);
            return _mapper.Map<AccountModel>(entity);
        }

        public void Save(AccountModel account)
        {
            var user = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;
            account.UserId = user.Id.ToString();
            _account.Save(_mapper.Map<Account>(account));
        }

        public void Delete(int accountId)
        {
            _account.Delete(accountId);
        }

        public AccountModel GetLast()
        {
            var account = _account.GetAll().OrderByDescending(x => x.Id).FirstOrDefault();
            return _mapper.Map<AccountModel>(account);
        }
    }
}
