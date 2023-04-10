using AutoMapper;
using BusinessLogic.Models;
using Trekster_web.Models;

namespace Trekster_web.Mapping
{
    public class CategoryVMProfile : Profile
    {
        public CategoryVMProfile()
        {
            CreateMap<CategoryModel, CategoryVM>().ReverseMap();
        }
    }
}
