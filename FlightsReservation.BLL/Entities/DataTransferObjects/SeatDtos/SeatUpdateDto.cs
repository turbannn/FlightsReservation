namespace FlightsReservation.BLL.Entities.DataTransferObjects.SeatDtos;

public class SeatUpdateDto : BaseSeatDto
{
    public Guid Id { get; set; }
    public bool IsAvailable { get; set; }
}