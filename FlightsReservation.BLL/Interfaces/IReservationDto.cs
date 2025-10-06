
namespace FlightsReservation.BLL.Interfaces;

public interface IReservationDto : IBaseTransferEntity
{
    string ReservationNumber { get; set; }
}