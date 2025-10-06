using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.SeatDtos;

public class SeatCreateDto : BaseTransferEntity, ISeatDto
{
    public string SeatNumber { get; set; }
}

