using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class TransactionModel : BaseModel
    {
        public DateTime Date { get; set; }

        public double Sum { get; set; }

        public virtual Account Account { get; set; }

        public virtual Currency Currency { get; set; }

        public virtual Category Category { get; set; }
    }
}
