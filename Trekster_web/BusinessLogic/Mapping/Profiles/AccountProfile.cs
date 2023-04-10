using AutoMapper;
using BusinessLogic.Models;
using Infrastructure.Entities;

namespace BusinessLogic.Mapping.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, AccountModel>().ReverseMap();
        }
    }
}
