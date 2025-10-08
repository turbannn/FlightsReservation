using FlightsReservation.BLL.DtoEntities.PassengerDtos;
using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.ReservationDtos;

public class ReservationCreateDto : BaseTransferEntity, IReservationDto
{
    public string ReservationNumber { get; set; } = null!;

    public Guid FlightId { get; set; }

    public List<PassengerCreateDto> Passengers { get; set; } = null!;
}