using BusinessLogic.Models;

namespace Trekster_web.Models
{
    public class SettingsVM
    {
        public AccountModel Account { get; set; } = new AccountModel();

        public IEnumerable<AccountModel> Accounts { get; set; } = new List<AccountModel>();

        public CategoryModel Category { get; set; } = new CategoryModel();

        public IEnumerable<CategoryModel> Categories { get; set; } = new List<CategoryModel>();

    }
}
