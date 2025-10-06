using AutoMapper;
using FlightsReservation.BLL.DtoEntities.FlightDtos;
using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.BLL.MapperProfiles;

public class FlightProfile : Profile
{
    FlightProfile()
    {
        CreateMap<Flight, FlightReadDto>().ForMember(dest => dest.AvailableSeats,
            opt =>
            {
                opt.MapFrom(src => src.Seats.Count(s => s.IsAvailable));
            });
        CreateMap<FlightCreateDto, Flight>();
        CreateMap<FlightUpdateDto, Flight>();
    }
}