using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using Novibet.EcbGateway.Models;
using Novibet.EcbGateway.Services;
using Autofac;
using Microsoft.Extensions.Configuration;
using Autofac.Extensions.DependencyInjection;
using Novibet.EcbGateway.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Novibet.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NovibetDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NovibetDb")));  //add dbcontext

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//using autofac as DI container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new EcbGatewayModule(builder.Configuration));  // using module for injection
});

builder.Services.AddControllers();//add controllers
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);  //add configuration to builder services

var app = builder.Build();

app.MapControllers();
app.Run();
