namespace FlightsReservation.BLL.DtoEntities.SeatDtos;

public class SeatUpdateDto : BaseSeatDto
{
    public Guid Id { get; set; }
    public bool IsAvailable { get; set; }
}