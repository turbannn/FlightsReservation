namespace FlightsReservation.DAL.Entities.Model;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
    public double Money {get; set;}

    public List<Reservation> Reservations { get; set; } = null!;
}