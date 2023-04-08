using Infrastructure.Entities;

namespace BusinessLogic.Models
{
    public class AccountModel : BaseModel
    {
        public string Name { get; set; }

        public virtual User User { get; set; }
    }
}
