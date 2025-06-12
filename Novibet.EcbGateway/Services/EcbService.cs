using Microsoft.Extensions.Configuration;
using System.Xml.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Novibet.EcbGateway.Models;


namespace Novibet.EcbGateway.Services
{
    public class EcbService : IEcbService
    {   
        private readonly HttpClient _httpClient;
        private readonly string _ecbUrl;

        public EcbService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _ecbUrl = configuration["EcbGateway:BaseUrl"];
            // Console.WriteLine($"ECB Base URL: {_ecbUrl}");
        }

        public async Task<List<CurrencyRate>> GetLatestRatesAsync()
        {
            var response = await _httpClient.GetStringAsync(_ecbUrl);
            return ParseXml(response);
        }

        
        private List<CurrencyRate> ParseXml(string xmlData)
        {
            var currencyRates = new List<CurrencyRate>();
            var document = XDocument.Parse(xmlData);
            var namespaces = document.Root.GetDefaultNamespace();
            var cubes = document.Descendants(namespaces + "Cube").Where(c => c.Attribute("currency") != null);
            
            foreach (var cube in cubes)
            {
                var currencyCode = cube.Attribute("currency")?.Value;
                var rate = decimal.TryParse(cube.Attribute("rate")?.Value, out var parsedRate) ? parsedRate : 0;
                
                currencyRates.Add(new CurrencyRate { CurrencyCode = currencyCode, Rate = rate });
            }

            return currencyRates;
        }
    }
}