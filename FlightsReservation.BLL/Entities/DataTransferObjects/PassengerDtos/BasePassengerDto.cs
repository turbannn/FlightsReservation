using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.PassengerDtos;

public class BasePassengerDto : IPassengerDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PassportNumber { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Guid SeatId { get; set; }
}