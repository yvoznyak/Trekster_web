using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Infrastructure.Entities;

namespace BusinessLogic.Services.ServiceInterfaces
{
    public interface ICurrencyService : IBase<CurrencyModel>
    {
        Currency GetByName(string name);
    }
}
