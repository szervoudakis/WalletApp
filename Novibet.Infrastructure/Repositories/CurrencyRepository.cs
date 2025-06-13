using Novibet.Infrastructure.Data;
using Novibet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Novibet.Infrastructure.Repositories
{
    public class CurrencyRepository
    {
        private readonly NovibetDbContext _context;

        public CurrencyRepository(NovibetDbContext context)
        {
            _context = context;

        }

        public async Task SaveCurrencyRatesAsync(List<Currency> currencies)
        {
             var newCurrencies = new List<Currency>();

            foreach (var currency in currencies)
            {
                var existingCurrency = await _context.Currencies.FirstOrDefaultAsync(c => c.CurrencyCode == currency.CurrencyCode && c.Date == currency.Date);
                if (existingCurrency == null)
                {
                    newCurrencies.Add(currency);  // add only the new currencies
                }
            }

            if (newCurrencies.Any()) // adding only new currencies
            {
                await _context.BulkInsertAsync(newCurrencies);
                // await _context.Bulk
            }
             
        }
        public async Task<List<Currency>> GetLatestRatesAsync()
        {
            return await _context.Currencies.ToListAsync();
        }
    }
}