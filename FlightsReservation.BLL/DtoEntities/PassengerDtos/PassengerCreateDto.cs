namespace FlightsReservation.BLL.DtoEntities.PassengerDtos;

public class PassengerCreateDto : BasePassengerDto
{
    public Guid ReservationId { get; set; }
}