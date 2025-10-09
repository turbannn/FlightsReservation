namespace FlightsReservation.BLL.Interfaces;

public interface IPassengerDto
{
    string FirstName { get; set; }
    string LastName { get; set; }
    string PassportNumber { get; set; }
    string PhoneNumber { get; set; }
    string Email { get; set; }

    Guid SeatId { get; set; }
}
