using AutoMapper;
using Novibet.EcbGateway.Models;
using Novibet.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Novibet.CurrencyApi.Mapping
{
    public class CurrencyMapper
    {
        private readonly IMapper _mapper;

        public CurrencyMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public List<Currency> MapToCurrency(List<CurrencyRate> rates)
        {
            return _mapper.Map<List<Currency>>(rates);  // using bulk mapping
        }
    }
}