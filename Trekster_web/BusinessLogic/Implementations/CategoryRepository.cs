using Infrastructure.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;

namespace BusinessLogic.Implementations
{
    public class CategoryRepository : ICategory
    {
        public AppDbContext context { get; set; }

        public CategoryRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return context.Categories;
        }

        public Category GetById(int categoryId)
        {
            return context.Categories.FirstOrDefault(x => x.Id == categoryId);
        }

        public void Save(Category category)
        {
            if (category.Id == default)
            {
                context.Entry(category).State = EntityState.Added;
            }
            else
            {
                context.Entry(category).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void Delete(int categoryId)
        {
            context.Transactions.RemoveRange(context.Transactions.Where(x => x.CategoryId == categoryId));
            context.Categories.Remove(new Category() { Id = categoryId });
            context.SaveChanges();
        }
    }
}
