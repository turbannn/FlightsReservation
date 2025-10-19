namespace FlightsReservation.DAL.Entities.Model;

public class Flight
{
    public Guid Id { get; set; }
    public string FlightNumber { get; set; } = null!;
    public string Departure { get; set; } = null!;
    public string Arrival { get; set; } = null!;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string AirplaneType { get; set; } = null!;
    public string Company { get; set; } = null!;
    public int Price { get; set; }
    public string Currency { get; set; } = null!;

    public List<Reservation> Reservations { get; set; } = null!;
    public List<Seat> Seats { get; set; } = null!;
}