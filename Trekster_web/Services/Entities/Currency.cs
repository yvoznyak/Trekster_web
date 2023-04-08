using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class Currency : BaseEntity
    {
        [Required]
        public string? Name { get; set; }

        public virtual ICollection<StartBalance>? StartBalances { get; set; }

        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
