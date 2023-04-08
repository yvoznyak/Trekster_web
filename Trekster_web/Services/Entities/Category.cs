using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [RegularExpression("^(1|-1)$")]
        public int Type { get; set; }

        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
