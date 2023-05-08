using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(
            ITransaction transaction,
            IMapper mapper,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            IAccountService account,
            ILogger<TransactionService> logger
        )
        {
            _transaction = transaction;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _account = account;
            _logger = logger;
        }

        public IEnumerable<TransactionModel> GetAll()
        {
            var entities = _transaction.GetAll();
            _logger.LogInformation($"Get All transactions");
            return _mapper.Map<List<TransactionModel>>(entities);
        }

        public IEnumerable<TransactionModel> GetAllForAccount(AccountModel accountModel)
        {
            _logger.LogInformation($"Get All For Account");
            var entities = _transaction.GetAll().Where(x => x.AccountId == accountModel.Id);
            return _mapper.Map<List<TransactionModel>>(entities);
        }

        public List<TransactionModel> GetAllForUser()
        {
            _logger.LogInformation($"Get All For User");
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
            _logger.LogInformation($"Get transactions with id={transactionId}");
            return _mapper.Map<TransactionModel>(entity);
        }

        public void Save(TransactionModel transaction)
        {
            _transaction.Save(_mapper.Map<Transaction>(transaction));
            _logger.LogInformation($"Save transactions with id={transaction.Id}");
        }

        public void Delete(int transactionId)
        {
            _logger.LogInformation($"Delete transactions with id={transactionId}");
            _transaction.Delete(transactionId);
        }

        public double GetFinalSum(int transactionId)
        {
            var transaction = _transaction.GetById(transactionId);
            _logger.LogInformation($"Final sum = {transaction.Category.Type * transaction.Sum} for transactions with id={transactionId}");
            return transaction.Category.Type * transaction.Sum;
        }
    }
}
