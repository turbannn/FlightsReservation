using System.ComponentModel.DataAnnotations.Schema;

namespace FlightsReservation.DAL.Entities.Model;

public class Seat
{
    public Guid Id { get; set; }
    public string SeatNumber { get; set; } = null!;
    public bool IsAvailable { get; set; } = true;

    public Passenger? Passenger { get; set; }

    public Guid FlightId { get; set; }
    [ForeignKey(nameof(FlightId))]
    public Flight Flight { get; set; } = null!;
}