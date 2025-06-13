using Autofac;
using AutoMapper;
using Novibet.CurrencyApi.Mapping;
using Novibet.Infrastructure.Repositories;
namespace Novibet.CurrencyApi.DependencyInjection
{
    public class CurrencyApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // register AutoMapper to Autofac
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CurrencyMappingProfile());
            }).CreateMapper()).As<IMapper>().SingleInstance();

            //register CurrencyMapper in DI Container
            builder.RegisterType<CurrencyMapper>().AsSelf().SingleInstance();

            //register currencyrepository in DI Container
            builder.RegisterType<CurrencyRepository>().AsSelf().InstancePerLifetimeScope();
        }
    }
}
