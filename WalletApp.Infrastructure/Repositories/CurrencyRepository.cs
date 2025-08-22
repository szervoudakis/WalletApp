using WalletApp.Infrastructure.Data;
using WalletApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using WalletApp.Application.Interfaces;

namespace WalletApp.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly NovibetDbContext _context;
        private readonly IConfiguration _configuration;
        public CurrencyRepository(NovibetDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        public async Task SaveCurrencyRatesAsync(List<Currency> currencies)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("NovibetDb"));
            await connection.OpenAsync();
            //bulk insert --
            var valuesList = new List<string>();
            var parameters = new DynamicParameters();
            var now = DateTime.UtcNow;

            for (int i = 0; i < currencies.Count; i++)
            {
                valuesList.Add($"(@p{i}_CurrencyCode, @p{i}_Rate, GETUTCDATE())");
                parameters.Add($"p{i}_CurrencyCode", currencies[i].CurrencyCode);
                parameters.Add($"p{i}_Rate", currencies[i].Rate);
                parameters.Add($"p{i}_Date", now);
            }
            var valuesClause = string.Join(", ", valuesList);

            var sql = $@"
            MERGE INTO Currencies AS target
            USING (VALUES
                {valuesClause}
            ) AS source (CurrencyCode, Rate, Date)
            ON target.CurrencyCode = source.CurrencyCode
            WHEN MATCHED THEN
                UPDATE SET target.Rate = source.Rate, target.Date = source.Date
            WHEN NOT MATCHED THEN
                INSERT (CurrencyCode, Rate, Date)
                VALUES (source.CurrencyCode, source.Rate, source.Date);
            ";

            await connection.ExecuteAsync(sql, parameters);
        }

        public async Task<List<Currency>> GetLatestRatesAsync()
        {
            return await _context.Currencies.ToListAsync();
        }

        public async Task UpdateAsync(Currency currency)
        {
            _context.Currencies.Update(currency);
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<string, decimal>> RetrieveCurrencyRateAsync(string currencyCode)
        {
            IQueryable<Currency> query = _context.Currencies;
            if (currencyCode != null && currencyCode.Length > 0)
            {
                query = query.Where(c => currencyCode.Contains(c.CurrencyCode));
            }
            var ratesList = await query.ToListAsync();
            return ratesList.ToDictionary(c => c.CurrencyCode, c => c.Rate);
        }
    }
}