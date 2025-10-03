namespace FlightsReservation.DAL.Entities.Model;

public class Flight
{
    public int Id { get; set; }
    public string FlightNumber { get; set; }
    public string Departure { get; set; }
    public string Arrival { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int AvailableSeats => Seats.Count(s => s.IsAvailable);
    public string AirplaneType { get; set; }

    public List<Reservation> Reservations { get; set; } = null!;
    public List<Seat> Seats { get; set; } = null!;
}