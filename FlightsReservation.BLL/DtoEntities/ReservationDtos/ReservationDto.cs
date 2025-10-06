using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.ReservationDtos;

public class ReservationDto : BaseTransferEntity, IReservationDto
{
    public string ReservationNumber { get; set; }
    public DateTime ReservationDate { get; set; }
}