
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightsReservation.DAL.Entities.Model;

public class Passenger
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PassportNumber { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string SeatNumber => Seat.SeatNumber;

    public int ReservationId { get; set; }
    [ForeignKey(nameof(ReservationId))]
    public Reservation Reservation { get; set; } = null!;

    //Dependent from seat
    public int SeatId { get; set; }
    [ForeignKey(nameof(SeatId))]
    public Seat Seat { get; set; } = null!;
}