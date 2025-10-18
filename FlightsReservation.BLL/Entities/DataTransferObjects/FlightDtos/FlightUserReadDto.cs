namespace FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;

public class FlightUserReadDto : BaseFlightDto
{
    public Guid Id { get; set; }
    public string FlightNumber { get; set; } = null!;
}