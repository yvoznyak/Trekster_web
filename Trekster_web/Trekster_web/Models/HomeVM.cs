namespace Trekster_web.Models
{
    public class HomeVM
    {
        public string? Summary { get; set; } = string.Empty;

        public double? ExpencesPercentage { get; set; } = 0;

        public double? ProfitsPercentage { get; set; } = 0;

        public bool ButtonExist { get; set; } = false;
    }
}
