using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Infrastructure.Entities;

namespace BusinessLogic.Services.ServiceInterfaces
{
    public interface IAccountService : IBase<AccountModel>
    {
        AccountModel GetLast();
    }
}
