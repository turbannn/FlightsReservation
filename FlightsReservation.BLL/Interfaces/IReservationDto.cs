
namespace FlightsReservation.BLL.Interfaces;

public interface IReservationDto
{
    string ReservationNumber { get; set; }

    Guid FlightId { get; set; }
}