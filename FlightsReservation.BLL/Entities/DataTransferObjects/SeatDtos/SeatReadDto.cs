namespace FlightsReservation.BLL.Entities.DataTransferObjects.SeatDtos;

public class SeatReadDto : BaseSeatDto
{
    public Guid Id { get; set; }
    public DateTime Lock { get; set; }
}