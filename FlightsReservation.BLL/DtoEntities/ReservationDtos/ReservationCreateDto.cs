using FlightsReservation.BLL.DtoEntities.PassengerDtos;
using System.Text.Json.Serialization;

namespace FlightsReservation.BLL.DtoEntities.ReservationDtos;

public class ReservationCreateDto : BaseReservationDto
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public List<PassengerCreateDto> Passengers { get; set; } = null!;
}