using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BusinessLogic.Services.ServiceImplementation
{
    public class TransactionService : ITransactionService
    {
        protected readonly ITransaction _transaction;
        protected readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountService _account;

        public TransactionService(ITransaction transaction,
                                  IMapper mapper,
                                  UserManager<User> userManager,
                                  IHttpContextAccessor httpContextAccessor,
                                  IAccountService account)
        {
            _transaction = transaction;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _account = account;
        }

        public IEnumerable<TransactionModel> GetAll()
        {
            var entities = _transaction.GetAll();
            return _mapper.Map<List<TransactionModel>>(entities);
        }

        public IEnumerable<TransactionModel> GetAllForAccount(AccountModel accountModel)
        {
            var entities = _transaction.GetAll().Where(x => x.AccountId == accountModel.Id);
            return _mapper.Map<List<TransactionModel>>(entities);
        }

        public List<TransactionModel> GetAllForUser()
        {
            var accounts = _account.GetAll();
            var transactions = new List<Transaction>();
            foreach (var account in accounts)
            {
                transactions.AddRange(_mapper.Map<List<Transaction>>(GetAllForAccount(account)));
            }

            return _mapper.Map<List<TransactionModel>>(transactions);
        }

        public TransactionModel GetById(int transactionId)
        {
            var entity = _transaction.GetById(transactionId);
            return _mapper.Map<TransactionModel>(entity);
        }

        public void Save(TransactionModel transaction)
        {
            _transaction.Save(_mapper.Map<Transaction>(transaction));
        }

        public void Delete(int transactionId)
        {
            _transaction.Delete(transactionId);
        }

        public double GetFinalSum(int transactionId)
        {
            var transaction = _transaction.GetById(transactionId);
            return transaction.Category.Type * transaction.Sum;
        }
    }
}
