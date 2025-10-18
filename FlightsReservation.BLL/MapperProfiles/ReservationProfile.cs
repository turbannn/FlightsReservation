using FlightsReservation.DAL.Entities.Model;
using AutoMapper;
using FlightsReservation.BLL.Entities.DataTransferObjects.ReservationDtos;

namespace FlightsReservation.BLL.MapperProfiles;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<Reservation, ReservationAdminReadDto>();
        CreateMap<Reservation, ReservationUserReadDto>();

        CreateMap<ReservationCreateDto, Reservation>();
    }
}