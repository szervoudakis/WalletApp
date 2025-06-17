using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using System;
using Novibet.Application.Interfaces;

public class CurrencyCacheService : ICurrencyCacheService
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _cache;

    private readonly IConfiguration _configuration;

    public CurrencyCacheService(IConfiguration configuration)
    {
        _configuration = configuration;
        _redis = ConnectionMultiplexer.Connect(InitializeRedis());
        _cache = _redis.GetDatabase();
    }

    private string InitializeRedis()
    {
        var redisConnectionString = _configuration.GetConnectionString("Redis")
        ?? throw new InvalidOperationException("Redis connection string is missing or empty.");
        return redisConnectionString;
    }

    public bool IsConnected() => _redis.IsConnected;

    public async Task<string> PingRedisAsync()
    {
        var result = await _cache.StringGetAsync("test_key");
        return result.IsNullOrEmpty ? "Redis is connected" : result.ToString();
    }

    public async Task SetCurrencyRatesAsync(Dictionary<string, decimal> rates)
    {
        foreach (var rate in rates)
        {
            await _cache.StringSetAsync(rate.Key, rate.Value.ToString());//store rates in cache
        }
        var codes = string.Join(",", rates.Keys);//all currency codes in cache
        await _cache.StringSetAsync("currency_codes", codes); //we want to know the updated currency codes (if ECB add new currency code)
    }

    public async Task<Dictionary<string, decimal>> GetCachedCurrencyRatesAsync()
    {
        var cachedCodes = await _cache.StringGetAsync("currency_codes");  //get currency codes from cache
        var result = new Dictionary<string, decimal>();
        if (cachedCodes.IsNullOrEmpty)
        {
            return result; //empty dictionary if we dont have cached data
        }

        var keys = cachedCodes.ToString().Split(",").Select(k => k.Trim()).ToArray(); //we use trim for posible spaces in redis storage for currency codes

        foreach (var key in keys)
        {
            var value = await _cache.StringGetAsync(key); //find specific currency code in cached data
            if (!value.IsNullOrEmpty)
            {
                result[key] = decimal.Parse(value!);
            }
        }
        //return in dictionary the cached data
        return result;
    }

    public async Task<decimal?> GetCachedRateAsync(string fromCurrency, string toCurrency)
    {
        var rates = await GetCachedCurrencyRatesAsync();
        var key = $"{fromCurrency}-{toCurrency}";
        if (rates.TryGetValue(key, out var rate))
        {
            return rate;
        }
        return null;
    }

}