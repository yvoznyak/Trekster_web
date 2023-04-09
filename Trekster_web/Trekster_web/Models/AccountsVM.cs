using BusinessLogic.Models;

namespace Trekster_web.Models
{
    public class AccountsVM
    {
        public string? Name { get; set; } = string.Empty;

        public Dictionary<string, float> StartBalances { get; set; } = new Dictionary<string, float>();

        public List<string> AccountsSummary { get; set; } = new List<string>();
    }
}
