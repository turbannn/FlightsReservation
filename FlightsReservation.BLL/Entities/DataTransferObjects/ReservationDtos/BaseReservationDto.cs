using FlightsReservation.BLL.Interfaces;
using System.Text.Json.Serialization;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.ReservationDtos;

public class BaseReservationDto : IReservationDto
{
    [JsonIgnore]
    public string ReservationNumber { get; set; } = null!;
    public Guid FlightId { get; set; }
}

