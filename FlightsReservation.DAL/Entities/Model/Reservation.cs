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

    public Guid? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public List<Passenger> Passengers { get; set; } = null!;
}