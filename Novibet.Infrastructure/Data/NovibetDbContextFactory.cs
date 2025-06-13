using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace Novibet.Infrastructure.Data
{
    public class NovibetDbContextFactory : IDesignTimeDbContextFactory<NovibetDbContext>
    {
        public NovibetDbContext CreateDbContext(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var parentDirectory = Directory.GetParent(currentDirectory);

            //checking id parentdirectory exist
            if (parentDirectory == null)
            {
                throw new InvalidOperationException("Parent directory not found.");
            }
                
            var basePath = Path.Combine(parentDirectory.FullName, "Novibet.CurrencyApi");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json",optional:false,reloadOnChange:true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<NovibetDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("NovibetDb"));

            return new NovibetDbContext(optionsBuilder.Options);
        }
    }
}
