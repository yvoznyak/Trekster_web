using System.ComponentModel.DataAnnotations;

namespace Trekster_web.Models
{
    public class TransactionVM
    {
        public int Id { get; set; } = 0;

        public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid number")]
        public double Sum { get; set; } = 0;

        public int AccountId { get; set; } = 0;

        public int CurrencyId { get; set; } = 0;

        public int CategoryId { get; set; } = 0;

        public string AccountsAndCurency { get; set; } = string.Empty;
    }
}
