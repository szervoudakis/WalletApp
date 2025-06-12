using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using Novibet.EcbGateway.Models;
using Novibet.EcbGateway.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();//add controllers
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);  //add configuration to builder services
builder.Services.AddHttpClient<IEcbService, EcbService>();  //add EcbService in DI 

var app = builder.Build();

app.MapControllers();
app.Run();
