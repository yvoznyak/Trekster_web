using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    public class Account : BaseEntity
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User? User { get; set; }

        public virtual ICollection<StartBalance>? StartBalances { get; set; }

        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
