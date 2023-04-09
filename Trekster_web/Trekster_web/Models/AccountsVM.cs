namespace Trekster_web.Models
{
    public class AccountsVM
    {
        public string? Name { get; set; }

        public Dictionary<string, float> StartBalances { get; set; }

        public List<string> AccountsSummary { get; set; }
    }
}
