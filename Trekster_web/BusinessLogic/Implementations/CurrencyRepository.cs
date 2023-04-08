using BusinessLogic.Interfaces;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace BusinessLogic.Implementations
{
    public class CurrencyRepository : ICurrency
    {
        public AppDbContext context { get; set; }

        public CurrencyRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Currency> GetAll()
        {
            return context.Currencies;
        }

        public Currency GetById(int currencyId)
        {
            return context.Currencies.FirstOrDefault(x => x.Id == currencyId);
        }

        public void Save(Currency currency)
        {
            if (currency.Id == default)
            {
                context.Entry(currency).State = EntityState.Added;
            }
            else
            {
                context.Entry(currency).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void Delete(int currencyId)
        {
            context.Transactions.RemoveRange(context.Transactions.Where(x => x.CurrencyId == currencyId));
            context.StartBalances.RemoveRange(context.StartBalances.Where(x => x.CurrencyId == currencyId));
            context.Currencies.Remove(new Currency() { Id = currencyId });
            context.SaveChanges();
        }
    }
}
