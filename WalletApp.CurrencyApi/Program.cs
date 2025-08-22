using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using WalletApp.EcbGateway.Models;
using WalletApp.EcbGateway.Services;
using Autofac;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Autofac.Extensions.DependencyInjection;
using WalletApp.EcbGateway.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WalletApp.Infrastructure.Data;
using WalletApp.CurrencyApi.DependencyInjection;
using WalletApp.CurrencyApi.Jobs;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WalletApp.CurrencyApi.Middleware;
using WalletApp.Infrastructure.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(config => 
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("NovibetDb")));
builder.Services.AddHangfireServer();

builder.Services.AddDbContext<NovibetDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NovibetDb")));  //add dbcontext

//using autofac as DI container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new EcbGatewayModule(builder.Configuration));  // using ecbgatewaymodule for injection
    containerBuilder.RegisterModule(new CurrencyApiModule());  //using currencyapimodule for injection
    containerBuilder.RegisterModule(new InfrastructureModule());
});

builder.Services.AddControllers();//add controllers
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);  //add configuration to builder services

var jwtsecret = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing in configuration.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsecret))
        };
    });

var app = builder.Build();
// app.UseIpRateLimiting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHangfireDashboard();
app.UseMiddleware<ExceptionHandlingMiddleware>();
//todo special class for scheduler
RecurringJob.AddOrUpdate<UpdateCurrencyRatesJob>(
    "update-currency-rates",
    job => job.Execute(),
    Cron.Minutely);

app.Run();
