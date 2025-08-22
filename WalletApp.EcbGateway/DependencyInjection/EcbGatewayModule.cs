using Autofac;
using Microsoft.Extensions.Configuration;
using WalletApp.EcbGateway.Services;
using System.Net.Http;
namespace WalletApp.EcbGateway.DependencyInjection
{
    public class EcbGatewayModule : Module
    {
        private readonly IConfiguration _configuration;

        public EcbGatewayModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_configuration).As<IConfiguration>().SingleInstance();
            builder.RegisterType<HttpClient>().AsSelf().SingleInstance();
            builder.RegisterType<EcbGatewayService>()
                   .As<IEcbGatewayService>()
                   .WithParameter("httpClient", new HttpClient())
                   .WithParameter("configuration", _configuration)
                   .SingleInstance();
        }
    }
}