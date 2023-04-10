using BusinessLogic.Models;

namespace Trekster_web.Models
{
    public class HistoryVM
    {
        public IEnumerable<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();

        public Dictionary<int, string> TransactionInfo { get; set; } = new Dictionary<int, string>();
    }
}
