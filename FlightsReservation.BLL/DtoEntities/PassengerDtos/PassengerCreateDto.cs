using System.Text.Json.Serialization;

namespace FlightsReservation.BLL.DtoEntities.PassengerDtos;

public class PassengerCreateDto : BasePassengerDto
{
    [JsonIgnore]
    public Guid ReservationId { get; set; }
}