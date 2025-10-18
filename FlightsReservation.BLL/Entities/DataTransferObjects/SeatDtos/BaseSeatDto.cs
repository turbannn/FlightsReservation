using FlightsReservation.BLL.Interfaces.Dtos;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.SeatDtos;

public class BaseSeatDto : ISeatDto
{
    public string SeatNumber { get; set; } = null!;
    public Guid FlightId { get; set; }
}