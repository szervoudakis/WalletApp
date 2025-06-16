
using AutoMapper;
using Novibet.Application.DTOs;
using Novibet.EcbGateway.Models; 
using Novibet.Domain.Entities;

public class CurrencyMappingProfile : Profile
{
    public CurrencyMappingProfile()
    {
        CreateMap<CurrencyRate, CurrencyRateDto>(); 
        CreateMap<CurrencyRateDto, Currency>(); 
    }
}
