﻿using Infrastructure.Entities;
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
    public class TransactionRepository : ITransaction
    {
        public AppDbContext context { get; set; }

        public TransactionRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Transaction> GetAll()
        {
            return context.Transactions.Include(x => x.Account).
                                        Include(x => x.Currency).
                                        Include(x => x.Category);
        }

        public Transaction GetById(int transactionId)
        {
            return context.Transactions.Include(x => x.Account).
                                        Include(x => x.Currency).
                                        Include(x => x.Category).FirstOrDefault(x => x.Id == transactionId);
        }

        public void Save(Transaction transaction)
        {
            if (transaction.Id == default)
            {
                context.Entry(transaction).State = EntityState.Added;
            }
            else
            {
                context.Entry(transaction).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void Delete(int transactionId)
        {
            context.Transactions.Remove(new Transaction() { Id = transactionId });
            context.SaveChanges();
        }
    }
}
