using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.PassengerDtos;

public class PassengerUpdateDto : BaseTransferEntity, IPassengerDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PassportNumber { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}