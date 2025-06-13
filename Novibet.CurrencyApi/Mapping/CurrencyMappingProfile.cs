using AutoMapper;
using Novibet.EcbGateway.Models;
using Novibet.Domain.Entities;

//we create profile because automapper uses mapping profiles 
namespace Novibet.CurrencyApi.Mapping
{
    public class CurrencyMappingProfile : Profile
    {
        public CurrencyMappingProfile()
        {
            CreateMap<CurrencyRate, Currency>();
        }
    }
}
