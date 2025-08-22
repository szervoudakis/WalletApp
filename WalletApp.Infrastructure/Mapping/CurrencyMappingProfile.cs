
using AutoMapper;
using WalletApp.Application.DTOs;
using WalletApp.EcbGateway.Models; 
using WalletApp.Domain.Entities;

public class CurrencyMappingProfile : Profile
{
    public CurrencyMappingProfile()
    {
        CreateMap<CurrencyRate, CurrencyRateDto>(); 
        CreateMap<CurrencyRateDto, Currency>(); 
    }
}
