namespace FlightsReservation.BLL.Interfaces.Dtos;

public interface ISeatDto
{
    string SeatNumber { get; set; }
    Guid FlightId { get; set; }
}