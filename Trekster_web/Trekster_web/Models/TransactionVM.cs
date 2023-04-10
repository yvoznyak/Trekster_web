using Infrastructure.Entities;

namespace Trekster_web.Models
{
    public class TransactionVM
    {
        public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();

        public double Sum { get; set; } = 0;

        public int AccountId { get; set; } = 0;

        public int CurrencyId { get; set; } = 0;

        public int CategoryId { get; set; } = 0;

        public string AccountsAndCurency { get; set; } = string.Empty;
    }
}
