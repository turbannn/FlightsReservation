using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.SeatsDtos;

public class SeatCreateDto : ISeatDto
{
    public int Id { get; set; }
    public string SeatNumber { get; set; }
}

