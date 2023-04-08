using Infrastructure.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;

namespace BusinessLogic.Implementations
{
    public class StartBalanceRepository : IStartBalance
    {
        public AppDbContext context { get; set; }

        public StartBalanceRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<StartBalance> GetAll()
        {
            return context.StartBalances.Include(x => x.Currency).
                                         Include(x => x.Account);
        }

        public StartBalance GetById(int startBalanceId)
        {
            return context.StartBalances.Include(x => x.Currency).
                                         Include(x => x.Account).FirstOrDefault(x => x.Id == startBalanceId);
        }

        public void Save(StartBalance startBalance)
        {
            if (startBalance.Id == default)
            {
                context.Entry(startBalance).State = EntityState.Added;
            }
            else
            {
                context.Entry(startBalance).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void Delete(int startBalanceId)
        {
            var tmpStartBalance = this.GetById(startBalanceId);
            context.Transactions.RemoveRange(context.Transactions.Where(x => x.CurrencyId == tmpStartBalance.CurrencyId && x.AccountId == tmpStartBalance.AccountId));
            context.StartBalances.Remove(new StartBalance() { Id = startBalanceId });
            context.SaveChanges();
        }
    }
}
