using AutoMapper;
using FlightsReservation.BLL.DtoEntities.PassengerDtos;
using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.BLL.MapperProfiles;

public class PassengerProfile : Profile
{
    public PassengerProfile()
    {
        CreateMap<Passenger, PassengerReadDto>().ForMember(dest => dest.SeatNumber,
            opt =>
            {
                opt.MapFrom(src => src.Seat.SeatNumber);
            });

        CreateMap<PassengerCreateDto, Passenger>();
        CreateMap<PassengerUpdateDto, Passenger>();
    }
}