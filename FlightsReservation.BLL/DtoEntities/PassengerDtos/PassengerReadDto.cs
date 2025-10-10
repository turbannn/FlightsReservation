namespace FlightsReservation.BLL.DtoEntities.PassengerDtos;

public class PassengerReadDto : BasePassengerDto
{
    public Guid Id { get; set; }
    public string SeatNumber { get; set; } = null!;
}