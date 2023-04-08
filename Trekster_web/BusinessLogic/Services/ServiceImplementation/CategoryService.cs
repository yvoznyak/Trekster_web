using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.ServiceImplementation
{
    public class CategoryService : ICategoryService
    {
        protected readonly ICategory _category;
        protected readonly IMapper _mapper;

        public CategoryService(ICategory category, IMapper mapper)
        {
            _category = category;
            _mapper = mapper;
        }

        public IEnumerable<CategoryModel> GetAll()
        {
            var entities = _category.GetAll();
            return _mapper.Map<List<CategoryModel>>(entities);
        }

        public CategoryModel GetById(int categoryId)
        {
            var entity = _category.GetById(categoryId);
            return _mapper.Map<CategoryModel>(entity);
        }

        public void Save(CategoryModel category)
        {
            _category.Save(_mapper.Map<Category>(category));
        }

        public void Delete(int categoryId)
        {
            _category.Delete(categoryId);
        }
    }
}
