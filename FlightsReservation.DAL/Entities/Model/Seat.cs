using System.ComponentModel.DataAnnotations.Schema;

namespace FlightsReservation.DAL.Entities.Model;

public class Seat
{
    public int Id { get; set; }
    public string SeatNumber { get; set; }
    public bool IsAvailable { get; set; } = true;

    public Passenger? Passenger { get; set; }

    public int FlightId { get; set; }
    [ForeignKey(nameof(FlightId))]
    public Flight Flight { get; set; } = null!;
}