using FlightsReservation.BLL.DtoEntities.PassengerDtos;
using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.ReservationDtos;

public class ReservationReadDto : BaseTransferEntity, IReservationDto
{
    public string ReservationNumber { get; set; } = null!;
    public DateTime ReservationDate { get; set; }

    public Guid FlightId { get; set; }

    public List<PassengerReadDto> Passengers { get; set; } = new();
}