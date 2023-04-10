using AutoMapper;
using BusinessLogic.Models;
using Trekster_web.Models;

namespace Trekster_web.Mapping
{
    public class AccountsVMProfile : Profile
    {
        public AccountsVMProfile()
        {
            CreateMap<AccountModel, AccountsVM>().ReverseMap();
        }
    }
}
