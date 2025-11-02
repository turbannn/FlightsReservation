using AutoMapper;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.Services.UtilityServices.FlightsApi;
using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.BLL.MapperProfiles.Resolvers;

public class ArrivalCityResolver : IValueResolver<AviationStackFlight, Flight, string>
{
    private readonly AirportCodeMapper _airportCodeMapper;

    public ArrivalCityResolver(AirportCodeMapper airportCodeMapper)
    {
        _airportCodeMapper = airportCodeMapper;
    }

    public string Resolve(AviationStackFlight source, Flight destination, string destMember, ResolutionContext context)
    {
        return _airportCodeMapper.GetCityName(source.Arrival.IataCode ?? source.Arrival.IcaoCode);
    }
}
