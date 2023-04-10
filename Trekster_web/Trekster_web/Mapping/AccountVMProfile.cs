using AutoMapper;
using BusinessLogic.Models;
using Trekster_web.Models;

namespace Trekster_web.Mapping
{
    public class AccountVMProfile : Profile
    {
        public AccountVMProfile()
        {
            CreateMap<AccountModel, AccountVM>().ReverseMap();
        }
    }
}
