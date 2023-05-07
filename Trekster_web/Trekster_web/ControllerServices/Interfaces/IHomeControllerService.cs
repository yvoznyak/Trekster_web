using Microsoft.AspNetCore.Mvc.Rendering;
using Trekster_web.Models;

namespace Trekster_web.ControllerServices.Interfaces
{
    public interface IHomeControllerService
    {
        SelectList GetListOfAccounts();

        void SaveTransaction(TransactionVM transactionVM);

        string GetSummary();

        double GetExpencesPercentage();

        bool ButtonExist();
    }
}
