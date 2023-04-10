using System.ComponentModel.DataAnnotations;

namespace Trekster_web.Models
{
    public class CategoryVM
    {
        public int Id { get; set; } = 0;

        public string Name { get; set; } = string.Empty;

        public int Type { get; set; } = 1;
    }
}
