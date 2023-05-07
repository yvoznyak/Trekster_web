using Microsoft.AspNetCore.Mvc.Rendering;
using Trekster_web.Models;

namespace Trekster_web.ControllerServices.Interfaces
{
    public interface IHistoryControllerService
    {
        Dictionary<int, Dictionary<string, string>> GetTransactionInfo();

        SelectList GetListOfAccounts();

        void SaveTransaction(TransactionVM transactionVM);
    }
}
