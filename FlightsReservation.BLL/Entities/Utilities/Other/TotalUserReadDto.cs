
using FlightsReservation.BLL.Entities.DataTransferObjects.ReservationDtos;

namespace FlightsReservation.BLL.Entities.Utilities.Other;

public class TotalUserReadDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int Money { get; set; }

    public List<ReservationUserReadDto> Reservations { get; set; } = null!;
}