
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using WalletApp.EcbGateway.Models;

namespace WalletApp.EcbGateway.Services
{
    public class EcbGatewayService : IEcbGatewayService
    {   
        private readonly HttpClient _httpClient;
        private readonly string? _ecbUrl;

        public EcbGatewayService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
           
            _ecbUrl = configuration["EcbGateway:BaseUrl"] ?? throw new ArgumentNullException(nameof(configuration), "ECB URL cannot be null");
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
            var namespaces = document.Root?.GetDefaultNamespace() ?? XNamespace.None; 

            // Attempt to extract date
            var dateAttribute = document.Descendants(namespaces + "Cube")
                                .FirstOrDefault(c => c.Attribute("time") != null)?
                                .Attribute("time")?.Value;

            // Try parse date safely
            var dateParsedSuccessfully = DateTime.TryParse(dateAttribute, out DateTime parsedDate);

            var cubes = document.Descendants(namespaces + "Cube")
                                .Where(c => c.Attribute("currency") != null && c.Attribute("rate") != null);

            foreach (var cube in cubes)
            {
                var currencyCode = cube.Attribute("currency")?.Value;
                var rateParsed = decimal.TryParse(cube.Attribute("rate")?.Value, out var parsedRate);

                // Safeguard against nulls and parse errors
                if (!string.IsNullOrEmpty(currencyCode) && rateParsed && dateParsedSuccessfully)
                {
                    currencyRates.Add(new CurrencyRate
                    {
                        CurrencyCode = currencyCode,
                        Rate = parsedRate,
                        Date = parsedDate
                    });
                }
            }

            return currencyRates;
        }

     }
}