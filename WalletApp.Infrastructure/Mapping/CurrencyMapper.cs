using AutoMapper;
using WalletApp.Domain.Entities;
using WalletApp.Application.DTOs;
namespace WalletApp.CurrencyApi.Mapping
{
    public class CurrencyMapper
    {
        private readonly IMapper _mapper;

        public CurrencyMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public List<Currency> MapToCurrency(List<CurrencyRateDto> rates)
        {
            return _mapper.Map<List<Currency>>(rates);  
        }
    }
}