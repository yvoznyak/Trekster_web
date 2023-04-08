using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class AccountModel : BaseModel
    {
        public string Name { get; set; }

        public virtual User User { get; set; }
    }
}
