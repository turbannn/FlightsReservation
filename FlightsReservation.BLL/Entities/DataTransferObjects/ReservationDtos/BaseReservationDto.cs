using FlightsReservation.BLL.Interfaces.Dtos;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.ReservationDtos;

public class BaseReservationDto : IReservationDto
{
    public Guid FlightId { get; set; }
}