using FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.ReservationDtos;

public class ReservationUserReadDto : BaseReservationDto
{
    public string ReservationNumber { get; set; } = null!;
    public DateTime ReservationDate { get; set; }
    public FlightUserReadDto Flight { get; set; } = null!;
}