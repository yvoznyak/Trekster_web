namespace Trekster_web.Models
{
    public class ExpencesVM
    {
        public List<string> ExpencesByCategory { get; set; } = new List<string>();

        public string? Summary { get; set; } = string.Empty;
    }
}
