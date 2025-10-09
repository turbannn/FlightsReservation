using FlightsReservation.BLL.DtoEntities.ReservationDtos;
using FlightsReservation.DAL.Entities.Model;
using AutoMapper;

namespace FlightsReservation.BLL.MapperProfiles;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<Reservation, ReservationReadDto>();

        CreateMap<ReservationCreateDto, Reservation>();
    }
}