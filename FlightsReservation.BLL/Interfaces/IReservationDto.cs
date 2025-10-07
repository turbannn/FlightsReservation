
namespace FlightsReservation.BLL.Interfaces;

public interface IReservationDto : IBaseTransferEntity
{
    string ReservationNumber { get; set; }
    DateTime ReservationDate { get; set; }
}