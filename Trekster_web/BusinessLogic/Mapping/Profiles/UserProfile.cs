using AutoMapper;
using BusinessLogic.Models;
using Infrastructure.Entities;

namespace BusinessLogic.Mapping.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap < UserModel, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
