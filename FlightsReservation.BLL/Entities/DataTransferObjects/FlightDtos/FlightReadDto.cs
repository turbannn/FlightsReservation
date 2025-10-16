using FlightsReservation.BLL.Entities.DataTransferObjects.SeatDtos;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;

public class FlightReadDto : BaseFlightDto
{
    public Guid Id { get; set; }
    public int AvailableSeats { get; set; }
    public List<SeatReadDto> Seats { get; set; } = null!;
}