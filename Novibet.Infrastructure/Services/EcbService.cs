using AutoMapper;
using Novibet.Application.Interfaces;
using Novibet.Application.DTOs;
using Novibet.EcbGateway.Services;

public class EcbService : IEcbService
{
    private readonly IEcbGatewayService  _ecbGateway;
    private readonly IMapper _mapper;

    public EcbService(IEcbGatewayService ecbGateway, IMapper mapper)
    {
        _ecbGateway = ecbGateway;
        _mapper = mapper;
    }

    public async Task<List<CurrencyRateDto>> GetLatestRatesAsync()
    {
        var rates = await _ecbGateway.GetLatestRatesAsync();
        return _mapper.Map<List<CurrencyRateDto>>(rates);
    }
}
