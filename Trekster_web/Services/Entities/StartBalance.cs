using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    public class StartBalance : BaseEntity
    {
        [Required]
        public double Sum { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        public virtual Account? Account { get; set; }

        public virtual Currency? Currency { get; set; }
    }
}
