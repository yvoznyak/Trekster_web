namespace BusinessLogic.Models
{
    public class TransactionModel : BaseModel
    {
        public DateTime Date { get; set; }

        public double Sum { get; set; }

        public int AccountId { get; set; }

        public int CurrencyId { get; set; }

        public int CategoryId { get; set; }
    }
}
