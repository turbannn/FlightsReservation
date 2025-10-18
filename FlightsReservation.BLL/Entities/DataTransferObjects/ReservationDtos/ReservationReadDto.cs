using FlightsReservation.BLL.Entities.DataTransferObjects.PassengerDtos;
using FlightsReservation.BLL.Interfaces.Dtos;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.ReservationDtos;

public class ReservationReadDto : BaseReservationDto, IReservationDto
{
    public DateTime ReservationDate { get; set; }
    public List<PassengerReadDto> Passengers { get; set; } = new();
}