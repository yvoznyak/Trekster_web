using BusinessLogic.Models;
using Infrastructure.Entities;

namespace Trekster_web.Models
{
    public class CategoryVM
    {
        public CategoryModel CategoryModel { get; set; } = new CategoryModel();

        public IEnumerable<CategoryModel> CategoryModels { get; set; } = new List<CategoryModel>();
    }
}
