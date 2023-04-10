using AutoMapper;
using BusinessLogic.Models;
using Infrastructure.Entities;

namespace BusinessLogic.Mapping.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryModel>().ReverseMap();
        }
    }
}
