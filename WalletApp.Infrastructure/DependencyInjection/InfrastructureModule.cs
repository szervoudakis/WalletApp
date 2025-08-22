using Autofac;
using WalletApp.Application.Interfaces;
using WalletApp.Infrastructure.Services;
using WalletApp.Infrastructure.Repositories;

namespace WalletApp.Infrastructure.DependencyInjection
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register EcbService
            builder.RegisterType<EcbService>().As<IEcbService>().InstancePerLifetimeScope();

            // Register CurrencyService , CurrencyRepository
            builder.RegisterType<CurrencyService>().As<ICurrencyService>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyRepository>().As<ICurrencyRepository>().InstancePerLifetimeScope();
        }
    }
}
