
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Novibet.EcbGateway.Models;
using Novibet.Infrastructure.Repositories;
using Novibet.Domain.Entities;

namespace Novibet.EcbGateway.Services
{
    public class EcbService : IEcbService
    {   
        private readonly HttpClient _httpClient;
        private readonly string _ecbUrl;

        private readonly CurrencyRepository _currencyRepository;

        public EcbService(HttpClient httpClient, IConfiguration configuration, CurrencyRepository currencyRepository)
        {
            _httpClient = httpClient;
            _currencyRepository = currencyRepository;
            _ecbUrl = configuration["EcbGateway:BaseUrl"];
        }

        public async Task<List<CurrencyRate>> GetLatestRatesAsync()
        {
            var response = await _httpClient.GetStringAsync(_ecbUrl);
            var rates = ParseXml(response);

            return rates;
        }

        
        private List<CurrencyRate> ParseXml(string xmlData)
        {
            var currencyRates = new List<CurrencyRate>();
            var document = XDocument.Parse(xmlData);
            var namespaces = document.Root.GetDefaultNamespace();
            
            //we found the date in xml
            var dateAttribute = document.Descendants(namespaces + "Cube")
                                .FirstOrDefault(c => c.Attribute("time") != null)?
                                .Attribute("time")?.Value;
            DateTime.TryParse(dateAttribute, out DateTime parsedDate);

            var cubes = document.Descendants(namespaces + "Cube").Where(c => c.Attribute("currency") != null);

            foreach (var cube in cubes)
            {
                var currencyCode = cube.Attribute("currency")?.Value;
                var rate = decimal.TryParse(cube.Attribute("rate")?.Value, out var parsedRate) ? parsedRate : 0;
                currencyRates.Add(new CurrencyRate { CurrencyCode = currencyCode, Rate = rate, Date = parsedDate });
            }

            return currencyRates;
        }
    }
}