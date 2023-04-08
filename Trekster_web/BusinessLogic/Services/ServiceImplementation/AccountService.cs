using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;

namespace BusinessLogic.Services.ServiceImplementation
{
    public class AccountService : IAccountService
    {
        private readonly IAccount _account;
        private readonly IMapper _mapper;

        public AccountService(IAccount account, IMapper mapper)
        {
            _account = account;
            _mapper = mapper;
        }

        public IEnumerable<AccountModel> GetAll()
        {
            var entities = _account.GetAll();
            return _mapper.Map<List<AccountModel>>(entities);
        }

        public AccountModel GetById(int accountId)
        {
            var entity = _account.GetById(accountId);
            return _mapper.Map<AccountModel>(entity);
        }

        public void Save(AccountModel account)
        {
            _account.Save(_mapper.Map<Account>(account));
        }

        public void Delete(int accountId)
        {
            _account.Delete(accountId);
        }

        public Account GetLast()
        {
            return _account.GetAll().OrderByDescending(x => x.Id).FirstOrDefault();
        }
    }
}
