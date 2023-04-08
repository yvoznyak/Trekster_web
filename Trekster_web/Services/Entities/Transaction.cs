using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    public class Transaction : BaseEntity
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double Sum { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public virtual Account? Account { get; set; }

        public virtual Currency? Currency { get; set; }

        public virtual Category? Category { get; set; }
    }
}
