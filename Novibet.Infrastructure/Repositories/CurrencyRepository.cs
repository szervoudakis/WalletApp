using Novibet.Infrastructure.Data;
using Novibet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Novibet.Application.Interfaces;

namespace Novibet.Infrastructure.Repositories
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

            var sql = @"
                MERGE INTO Currencies AS target
                USING (VALUES (@CurrencyCode, @Rate))
                AS source (CurrencyCode, Rate)
                ON target.CurrencyCode = source.CurrencyCode
                WHEN MATCHED THEN 
                    UPDATE SET target.Rate = source.Rate, target.Date = GETUTCDATE()
                WHEN NOT MATCHED THEN
                    INSERT (CurrencyCode, Rate, Date)
                    VALUES (source.CurrencyCode, source.Rate, GETUTCDATE());
            ";

            foreach (var currency in currencies)
            {
                await connection.ExecuteAsync(sql, new { currency.CurrencyCode, currency.Rate, Date = DateTime.UtcNow });
            }
        }

        public async Task<List<Currency>> GetLatestRatesAsync()
        {
            return await _context.Currencies.ToListAsync();
        }
    }
}