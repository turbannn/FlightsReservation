using System.ComponentModel.DataAnnotations.Schema;

namespace FlightsReservation.DAL.Entities.Model;

public class Reservation
{
    public Guid Id { get; set; }
    public string ReservationNumber { get; set; } = null!;
    public DateTime ReservationDate { get; set; }

    public Guid FlightId { get; set; }
    [ForeignKey(nameof(FlightId))]
    public Flight Flight { get; set; } = null!;

    public List<Passenger> Passengers { get; set; } = null!;
}