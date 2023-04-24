namespace Trekster_web.Models
{
    public class ProfitsVM
    {
        public List<string> ProfitsByCategory { get; set; } = new List<string>();

        public string? Summary { get; set; } = string.Empty;
    }
}
