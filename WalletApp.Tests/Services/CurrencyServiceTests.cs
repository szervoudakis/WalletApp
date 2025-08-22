using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletApp.Infrastructure.Services;
using WalletApp.Application.Interfaces;
using AutoMapper;
using WalletApp.Infrastructure.Repositories;

namespace WalletApp.Tests.Services
{
    public class CurrencyServiceTests
    {
        [Fact]
        public async Task ConvertAsync_ShouldConvertCorrectly_WhenRatesExist()
        {
            // Arrange
            var mockEcb = new Mock<IEcbService>();
            var mockRepo = new Mock<ICurrencyRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCache = new Mock<ICurrencyCacheService>();

            mockCache.Setup(x => x.GetCachedCurrencyRatesAsync())
                     .ReturnsAsync(new Dictionary<string, decimal>
                     {
                         { "USD", 1.2m },
                         { "EUR", 1.0m }
                     });

            var service = new CurrencyService(
                mockEcb.Object,
                mockRepo.Object,
                mockMapper.Object,
                mockCache.Object
            );

            // Act
            var result = await service.ConvertAsync(12m, "USD", "EUR");

            // Assert
            Assert.Equal(10m, result); // 12 / 1.2 = 10 EUR
        }
    }
}
