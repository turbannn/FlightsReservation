using AutoMapper;
using FlightsReservation.BLL.DtoEntities.SeatDtos;
using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.BLL.MapperProfiles;

public class SeatProfile : Profile
{
    public SeatProfile()
    {
        CreateMap<Seat, SeatReadDto>();
        
        CreateMap<SeatCreateDto, Seat>();
        CreateMap<SeatUpdateDto, Seat>();
    }
}