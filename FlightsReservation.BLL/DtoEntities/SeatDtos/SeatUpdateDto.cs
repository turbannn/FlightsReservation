using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.SeatDtos;

public class SeatUpdateDto : BaseTransferEntity, ISeatDto
{
    public string SeatNumber { get; set; }
    public bool IsAvailable { get; set; }
}

