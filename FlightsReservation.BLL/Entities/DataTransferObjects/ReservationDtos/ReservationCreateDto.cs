using FlightsReservation.BLL.Entities.DataTransferObjects.PassengerDtos;
using System.Text.Json.Serialization;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.ReservationDtos;

public class ReservationCreateDto : BaseReservationDto
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public List<PassengerCreateDto> Passengers { get; set; } = null!;
}