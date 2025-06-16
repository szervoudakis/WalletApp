using Xunit;
using Novibet.Infrastructure.Repositories;
using Novibet.Infrastructure.Data;
using Novibet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Novibet.Tests.Repositories
{
    public class WalletRepositoryTests
    {
        private NovibetDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<NovibetDbContext>()
                .UseInMemoryDatabase(databaseName: "WalletDb_Test")
                .Options;

            return new NovibetDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddWalletAndReturnId()
        {
            // Arrange
            var context = CreateDbContext();
            var repository = new WalletRepository(context);
            var wallet = new Wallet(1, "EUR");
            wallet.AddFunds(100.00m);

            // Act
            var createdId = await repository.CreateAsync(wallet);

            // Assert
            Assert.True(createdId > 0);
            var inserted = await context.Wallets.FindAsync(createdId);
            Assert.NotNull(inserted);
            Assert.Equal(100.00m, inserted!.Balance);
            Assert.Equal(1, inserted.Id);
        }

        [Fact]
        public async Task RetrieveWalletByIdAsync_ShouldReturnCorrectWallet()
        {
            // Arrange
            var context = CreateDbContext();
            var repository = new WalletRepository(context);
            var wallet = new Wallet(3, "USD");
            wallet.AddFunds(100.00m);
            await context.Wallets.AddAsync(wallet);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.RetrieveWalletByIdAsync(wallet.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(wallet.Id, result!.Id);
            Assert.Equal(wallet.Balance, result.Balance);
        }
    }
}
