using Trekster_web.Models;

namespace Trekster_web.ControllerServices.Interfaces
{
    public interface IAccountControllerService
    {
        List<string> GetAccountsInfo();

        bool StartBalancesNotEmpty(AccountsVM accountsVM);

        void SaveAccount(AccountsVM accountsVM);
    }
}
