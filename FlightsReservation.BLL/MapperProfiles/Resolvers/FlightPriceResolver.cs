using AutoMapper;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.Services.UtilityServices.FlightsApi;
using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.BLL.MapperProfiles.Resolvers;

public class FlightPriceResolver : IValueResolver<AviationStackFlight, Flight, double>
{
    private readonly PriceCounter _priceCounter;

    public FlightPriceResolver(PriceCounter priceCounter)
    {
        _priceCounter = priceCounter;
    }

    public double Resolve(AviationStackFlight source, Flight destination, double destMember, ResolutionContext context)
    {
        return _priceCounter.CalculatePrice(
            source.Departure.IataCode ?? source.Departure.IcaoCode ?? "",
            source.Arrival.IataCode ?? source.Arrival.IcaoCode ?? "",
            source.Aircraft?.ModelCode);
    }
}
