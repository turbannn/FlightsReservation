namespace FlightsReservation.BLL.Interfaces;

public interface ISeatDto : IBaseTransferEntity
{
    string SeatNumber { get; set; }
}