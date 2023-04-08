using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Infrastructure.Entities;

namespace BusinessLogic.Services.ServiceInterfaces
{
    public interface IAccountService : IBase<AccountModel>
    {
        Account GetLast();
    }
}
