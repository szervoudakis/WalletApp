using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using Novibet.Application.Commands.Wallets;
using Novibet.Application.Interfaces;

namespace Novibet.Application.DependencyInjection
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // register mediator and CQRS handlers
            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .AsClosedTypesOf(typeof(IRequestHandler<,>))
                   .InstancePerLifetimeScope();
                   
            //  builder.RegisterType<WalletRepository>().As<IWalletRepository>().InstancePerLifetimeScope();      

        }
    }
}