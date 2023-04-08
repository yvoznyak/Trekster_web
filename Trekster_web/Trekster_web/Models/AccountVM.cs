using BusinessLogic.Models;

namespace Trekster_web.Models
{
    public class AccountVM
    {
        public AccountModel Account { get; set; } = new AccountModel();

        public IEnumerable<AccountModel> Accounts { get; set; } = new List<AccountModel>();
    }
}
