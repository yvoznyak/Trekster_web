using System;
using AutoMapper;
using BusinessLogic.Services.ServiceImplementation;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Infrastructure.Entities;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.Extensions.Logging;

namespace TreksterTests
{
    namespace BusinessLogic.Tests.Services
    {
        public class CategoryServiceTests
        {
            private readonly Mock<ICategory> _mockCategoryRepository;
            private readonly IMapper _mapper;
            private readonly Mock<ILogger<CategoryService>> _loggerMock;

            public CategoryServiceTests()
            {
                _mockCategoryRepository = new Mock<ICategory>();
                var mapperConfig = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new AutoMapperProfile());
                });
                _mapper = mapperConfig.CreateMapper();
                _loggerMock = new Mock<ILogger<CategoryService>>();
            }

            [Fact]
            public void GetAll_ShouldReturnAllCategories()
            {
                // Arrange
                var categories = new List<Category>()
            {
                new Category() { Id = 1, Name = "Category 1", Type = 1 },
                new Category() { Id = 2, Name = "Category 2", Type = -1 },
                new Category() { Id = 3, Name = "Category 3", Type = 1 }
            };
                _mockCategoryRepository.Setup(repo => repo.GetAll()).Returns(categories);

                var categoryService = new CategoryService(_mockCategoryRepository.Object, _mapper, _loggerMock.Object);

                // Act
                var result = categoryService.GetAll();

                // Assert
                Assert.Equal(categories.Count, result.Count());
                foreach (var category in categories)
                {
                    var categoryModel = result.SingleOrDefault(c => c.Id == category.Id);
                    Assert.NotNull(categoryModel);
                    Assert.Equal(category.Name, categoryModel.Name);
                    Assert.Equal(category.Type, categoryModel.Type);
                }
            }

            [Fact]
            public void GetById_ShouldReturnCategoryByIdNotNull()
            {
                // Arrange
                var categoryId = 1;
                var category = new Category() { Id = categoryId, Name = "Category 1", Type = 1 };
                _mockCategoryRepository.Setup(repo => repo.GetById(categoryId)).Returns(category);

                var categoryService = new CategoryService(_mockCategoryRepository.Object, _mapper, _loggerMock.Object);

                // Act
                var result = categoryService.GetById(categoryId);

                // Assert
                Assert.NotNull(result);
            }

            [Fact]
            public void GetById_ShouldReturnCategoryByIdEqualName()
            {
                // Arrange
                var categoryId = 1;
                var category = new Category() { Id = categoryId, Name = "Category 1", Type = 1 };
                _mockCategoryRepository.Setup(repo => repo.GetById(categoryId)).Returns(category);

                var categoryService = new CategoryService(_mockCategoryRepository.Object, _mapper, _loggerMock.Object);

                // Act
                var result = categoryService.GetById(categoryId);

                // Assert
                Assert.Equal(category.Name, result.Name);
               
            }

            [Fact]
            public void GetById_ShouldReturnCategoryByIdEqualType()
            {
                // Arrange
                var categoryId = 1;
                var category = new Category() { Id = categoryId, Name = "Category 1", Type = 1 };
                _mockCategoryRepository.Setup(repo => repo.GetById(categoryId)).Returns(category);

                var categoryService = new CategoryService(_mockCategoryRepository.Object, _mapper, _loggerMock.Object);

                // Act
                var result = categoryService.GetById(categoryId);

                // Assert

                Assert.Equal(category.Type, result.Type);
            }

            [Fact]
            public void Delete_ShouldCallDeleteMethodOnRepository()
            {
                // Arrange
                var categoryId = 1;
                var categoryService = new CategoryService(_mockCategoryRepository.Object, _mapper, _loggerMock.Object);

                // Act
                categoryService.Delete(categoryId);

                // Assert
                _mockCategoryRepository.Verify(repo => repo.Delete(categoryId), Times.Once);
            }
        }
    }
}

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Category, CategoryModel>().ReverseMap();
    }
}