using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Infrastructure.Entities;

namespace BusinessLogic.Services.ServiceInterfaces
{
    public interface ICurrencyService : IBase<CurrencyModel>
    {
        CurrencyModel GetByName(string name);
        IEnumerable<CurrencyModel> GetAllByAccount(AccountModel accountModel);
    }
}
