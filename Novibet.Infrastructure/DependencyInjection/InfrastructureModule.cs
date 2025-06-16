using Autofac;
using Novibet.Application.Interfaces;
using Novibet.Infrastructure.Services;
using Novibet.Infrastructure.Repositories;

namespace Novibet.Infrastructure.DependencyInjection
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
