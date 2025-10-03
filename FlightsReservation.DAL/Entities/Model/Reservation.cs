
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightsReservation.DAL.Entities.Model;

public class Reservation
{
    public int Id { get; set; }
    public string ReservationNumber { get; set; }
    public DateTime ReservationDate { get; set; }

    public int FlightId { get; set; }
    [ForeignKey(nameof(FlightId))]
    public Flight Flight { get; set; } = null!;

    public List<Passenger> Passengers { get; set; } = null!;
}