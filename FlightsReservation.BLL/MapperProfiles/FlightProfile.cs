using FlightsReservation.DAL.Entities.Model;
using AutoMapper;
using FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.MapperProfiles.Resolvers;

namespace FlightsReservation.BLL.MapperProfiles;

public class FlightProfile : Profile
{
    public FlightProfile()
    {
        CreateMap<Flight, FlightAdminReadDto>().ForMember(dest => dest.AvailableSeats,
            opt =>
            {
                opt.MapFrom(src => src.Seats.Count(s => s.IsAvailable));
            });
        CreateMap<Flight, FlightUserReadDto>();

        CreateMap<FlightCreateDto, Flight>();
        CreateMap<FlightUpdateDto, Flight>();

        // Mapping from AviationStackFlight to Flight
        CreateMap<AviationStackFlight, Flight>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src =>
                src.Flight.Number ?? src.Flight.IcaoNumber ?? src.Flight.IataNumber ?? "UNKNOWN"))
            .ForMember(dest => dest.Departure, opt => opt.MapFrom<DepartureCityResolver>())
            .ForMember(dest => dest.Arrival, opt => opt.MapFrom<ArrivalCityResolver>())
            .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => 
                ParseDateTime(src.Departure.ScheduledTime)))
            .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => 
                ParseDateTime(src.Arrival.ScheduledTime)))
            .ForMember(dest => dest.AirplaneType, opt => opt.MapFrom(src => 
                src.Aircraft != null ? src.Aircraft.ModelText ?? "Unknown" : "Unknown"))
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => 
                src.Airline.Name ?? src.Airline.IataCode ?? "Unknown"))
            .ForMember(dest => dest.Price, opt => opt.MapFrom<FlightPriceResolver>())
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => "PLN"))
            .ForMember(dest => dest.Reservations, opt => opt.Ignore())
            .ForMember(dest => dest.Seats, opt => opt.Ignore());
    }

    private static DateTime ParseDateTime(string? timeString)
    {
        if (string.IsNullOrEmpty(timeString))
            return DateTime.UtcNow;

        if (TimeSpan.TryParse(timeString, out var time))
        {
            return DateTime.UtcNow.Date.Add(time);
        }

        return DateTime.UtcNow;
    }
}