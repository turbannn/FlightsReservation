using FlightsReservation.BLL.DtoEntities.PassengerDtos;
using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.ReservationDtos;

public class ReservationCreateDto : BaseReservationDto
{
    public Guid Id { get; set; }
    public List<PassengerCreateDto> Passengers { get; set; } = null!;
}