using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.SeatDtos;

public class SeatReadDto : BaseTransferEntity, ISeatDto
{
    public string SeatNumber { get; set; } = null!;
    public Guid FlightId { get; set; }
}