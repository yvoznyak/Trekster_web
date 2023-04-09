using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;

namespace BusinessLogic.Services.ServiceImplementation
{
    public class TransactionService : ITransactionService
    {
        protected readonly ITransaction _transaction;
        protected readonly IMapper _mapper;

        public TransactionService(ITransaction transaction, IMapper mapper)
        {
            _transaction = transaction;
            _mapper = mapper;
        }

        public IEnumerable<TransactionModel> GetAll()
        {
            var entities = _transaction.GetAll();
            return _mapper.Map<List<TransactionModel>>(entities);
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
