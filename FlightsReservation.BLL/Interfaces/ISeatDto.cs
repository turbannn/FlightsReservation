namespace FlightsReservation.BLL.Interfaces;

public interface ISeatDto : IBaseTransferEntity
{
    string SeatNumber { get; set; }
    Guid FlightId { get; set; }
}