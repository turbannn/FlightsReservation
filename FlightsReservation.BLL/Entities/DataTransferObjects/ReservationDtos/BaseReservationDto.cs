using System.Text.Json.Serialization;
using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.ReservationDtos;

public class BaseReservationDto : IReservationDto
{
    [JsonIgnore]
    public string ReservationNumber { get; set; } = null!;
    public Guid FlightId { get; set; }
}

