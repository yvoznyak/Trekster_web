using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.ServiceInterfaces
{
    public interface ITransactionService : IBase<TransactionModel>
    {
        double GetFinalSum(int transactionId);

        IEnumerable<TransactionModel> GetAllForAccount(AccountModel accountModel);

        List<TransactionModel> GetAllForUser();
    }
}
