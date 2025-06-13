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
            //for-loop for currecies
            foreach (var currency in currencies)
            {
                var existing_currency = await _context.Currencies.FirstOrDefaultAsync(c => c.CurrencyCode == currency.CurrencyCode && c.Date == currency.Date);
                if (existing_currency == null)
                {
                    _context.Currencies.Add(currency);
                }

                await _context.SaveChangesAsync();
            }
             
        }
        public async Task<List<Currency>> GetLatestRatesAsync()
        {
            return await _context.Currencies.ToListAsync();
        }
    }
}