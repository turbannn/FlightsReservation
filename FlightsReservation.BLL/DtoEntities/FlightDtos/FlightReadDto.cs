namespace FlightsReservation.BLL.DtoEntities.FlightDtos;

public class FlightReadDto : BaseFlightDto
{
    public Guid Id { get; set; }
    public int AvailableSeats { get; set; }
}