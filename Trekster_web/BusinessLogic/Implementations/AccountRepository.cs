using BusinessLogic.Interfaces;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Implementations
{
    public class AccountRepository : IAccount
    {
        public AppDbContext context { get; set; }

        public AccountRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Account> GetAll()
        {
            return context.Accounts.Include(x => x.User);
        }

        public Account GetById(int accountId)
        {
            return context.Accounts.Include(x => x.User).FirstOrDefault(x => x.Id == accountId);
        }

        public void Save(Account account)
        {
            account.User = context.Users.FirstOrDefault(x => x.Id == account.UserId);

            if (account.Id == default)
            {
                context.Entry(account).State = EntityState.Added;
            }
            else
            {
                context.Entry(account).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void Delete(int accountId)
        {
            context.Transactions.RemoveRange(context.Transactions.Where(x => x.AccountId == accountId));
            context.StartBalances.RemoveRange(context.StartBalances.Where(x => x.AccountId == accountId));
            context.Accounts.Remove(new Account() { Id = accountId });
            context.SaveChanges();
        }
    }
}
