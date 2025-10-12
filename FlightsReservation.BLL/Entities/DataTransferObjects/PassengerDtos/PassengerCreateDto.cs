using System.Text.Json.Serialization;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.PassengerDtos;

public class PassengerCreateDto : BasePassengerDto
{
    [JsonIgnore]
    public Guid ReservationId { get; set; }
}