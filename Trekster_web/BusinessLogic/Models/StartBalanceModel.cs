using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class StartBalanceModel : BaseModel
    {
        public double Sum { get; set; }

        public virtual Account Account { get; set; }

        public virtual Currency Currency { get; set; }
    }
}
