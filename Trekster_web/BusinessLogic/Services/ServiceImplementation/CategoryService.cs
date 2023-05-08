using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategory category,
            IMapper mapper,
            ILogger<CategoryService> logger
        )
        {
            _category = category;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<CategoryModel> GetAll()
        {
            var entities = _category.GetAll();
            _logger.LogInformation($"Get all categories");
            return _mapper.Map<List<CategoryModel>>(entities);
        }

        public CategoryModel GetById(int categoryId)
        {
            var entity = _category.GetById(categoryId);
            _logger.LogInformation($"Get category with id={categoryId}.");
            return _mapper.Map<CategoryModel>(entity);
        }

        public void Save(CategoryModel category)
        {
            _category.Save(_mapper.Map<Category>(category));
            _logger.LogInformation($"Category saved with name={category.Name}.");
        }

        public void Delete(int categoryId)
        {
            _category.Delete(categoryId);
            _logger.LogInformation($"Category deleted with id={categoryId}.");
        }
    }
}
