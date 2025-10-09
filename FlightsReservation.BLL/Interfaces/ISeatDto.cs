namespace FlightsReservation.BLL.Interfaces;

public interface ISeatDto
{
    string SeatNumber { get; set; }
    Guid FlightId { get; set; }
}