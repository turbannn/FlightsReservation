using FlightsReservation.DAL.Entities.Model;
using AutoMapper;
using FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;

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
    }
}