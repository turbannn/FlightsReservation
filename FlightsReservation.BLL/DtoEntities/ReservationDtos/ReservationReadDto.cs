using FlightsReservation.BLL.DtoEntities.PassengerDtos;
using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.ReservationDtos;

public class ReservationReadDto : BaseReservationDto, IReservationDto
{
    public DateTime ReservationDate { get; set; }
    public List<PassengerReadDto> Passengers { get; set; } = new();
}