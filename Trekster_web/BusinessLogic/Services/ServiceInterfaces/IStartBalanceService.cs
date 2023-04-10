using BusinessLogic.Interfaces;
using BusinessLogic.Models;

namespace BusinessLogic.Services.ServiceInterfaces
{
    public interface IStartBalanceService : IBase<StartBalanceModel>
    {
        IEnumerable<StartBalanceModel> GetAllForAccount(AccountModel accountModel);
    }
}
