using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities
{
    public class User : IdentityUser
    {
        public virtual ICollection<Account>? Accounts { get; set; }
    }
}
