using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.ReservationDtos;

public class BaseReservationDto : IReservationDto
{
    public string ReservationNumber { get; set; } = null!;
    public Guid FlightId { get; set; }
}

