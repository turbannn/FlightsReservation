using AutoMapper;
using FlightsReservation.BLL.DtoEntities.ReservationDtos;
using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.BLL.MapperProfiles;

public class ReservationProfile : Profile
{
    ReservationProfile()
    {
        CreateMap<Reservation, ReservationDto>();

        CreateMap<ReservationDto, Reservation>();
    }
}

