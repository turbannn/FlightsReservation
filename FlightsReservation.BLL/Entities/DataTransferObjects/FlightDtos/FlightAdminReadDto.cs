using FlightsReservation.BLL.Entities.DataTransferObjects.SeatDtos;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;

public class FlightAdminReadDto : BaseFlightDto
{
    public Guid Id { get; set; }
    public int AvailableSeats { get; set; }
    public string FlightNumber { get; set; } = null!;
    public List<SeatReadDto> Seats { get; set; } = null!;
}