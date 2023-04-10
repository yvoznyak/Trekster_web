using BusinessLogic.Models;

namespace Trekster_web.Models
{
    public class SettingsVM
    {
        public IEnumerable<AccountModel> Accounts { get; set; } = new List<AccountModel>();

        public IEnumerable<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
    }
}
