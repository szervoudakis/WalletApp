using AutoMapper;
using Novibet.Domain.Entities;
using Novibet.Application.DTOs;
namespace Novibet.CurrencyApi.Mapping
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