using Autofac;
using AutoMapper;
using Hangfire;
using MediatR;
using Hangfire.SqlServer;
using Novibet.CurrencyApi.Mapping;
using Novibet.Infrastructure.Repositories;
using Novibet.Infrastructure.Data;
using Novibet.CurrencyApi.Jobs;
using Novibet.Application.Interfaces;
using Novibet.Application.Commands.Wallets;
using Novibet.Infrastructure.DependencyInjection;

namespace Novibet.CurrencyApi.DependencyInjection
{
    public class CurrencyApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 1) register AutoMapper to Autofac
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CurrencyMappingProfile());
            }).CreateMapper()).As<IMapper>().SingleInstance();

            //2) register CurrencyMapper in DI Container
            builder.RegisterType<CurrencyMapper>().AsSelf().SingleInstance();

            //3) register currencyrepository in DI Container
            builder.RegisterType<CurrencyRepository>().AsSelf().InstancePerLifetimeScope();

            //4) register novibetDbContext in DI
            builder.RegisterType<NovibetDbContext>().AsSelf().InstancePerLifetimeScope();

            //5) hangfire with sql server storage
            builder.Register(c => new SqlServerStorage(c.Resolve<IConfiguration>().GetConnectionString("NovibetDb")))
            .AsSelf()
            .SingleInstance();

            //6) register updatecurrencyratesjob in DI Container
            builder.RegisterType<UpdateCurrencyRatesJob>().AsSelf().InstancePerLifetimeScope();

            //7) register CurrencyCacheService in DI Container
            builder.RegisterType<CurrencyCacheService>().As<ICurrencyCacheService>().SingleInstance();

            //8) register WalletRepository in DI Container
            builder.RegisterType<WalletRepository>().As<IWalletRepository>().InstancePerLifetimeScope();

            //9) register MediatR and CQRS handlers in DI container
            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(CreateWalletCommand).Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>))
            .InstancePerLifetimeScope();
            
            // //10)
            // builder.RegisterModule<InfrastructureModule>(); //register Infrastructure services

        }
    }
}
