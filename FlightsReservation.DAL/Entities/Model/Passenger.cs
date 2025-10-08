using System.ComponentModel.DataAnnotations.Schema;

namespace FlightsReservation.DAL.Entities.Model;

public class Passenger
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PassportNumber { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;

    public Guid ReservationId { get; set; }
    [ForeignKey(nameof(ReservationId))]
    public Reservation Reservation { get; set; } = null!;

    //Dependent from seat
    public Guid SeatId { get; set; }
    [ForeignKey(nameof(SeatId))]
    public Seat Seat { get; set; } = null!;
}