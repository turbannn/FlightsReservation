namespace FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;

public class FlightReadDto : BaseFlightDto
{
    public Guid Id { get; set; }
    public int AvailableSeats { get; set; }
}