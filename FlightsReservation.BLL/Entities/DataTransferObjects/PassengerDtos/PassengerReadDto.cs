namespace FlightsReservation.BLL.Entities.DataTransferObjects.PassengerDtos;

public class PassengerReadDto : BasePassengerDto
{
    public Guid Id { get; set; }
    public string SeatNumber { get; set; } = null!;
}