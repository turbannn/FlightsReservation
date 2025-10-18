namespace FlightsReservation.BLL.Interfaces.Dtos;

public interface IReservationDto
{
    string ReservationNumber { get; set; }

    Guid FlightId { get; set; }
}