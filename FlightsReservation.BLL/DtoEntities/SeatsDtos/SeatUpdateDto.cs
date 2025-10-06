using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.SeatsDtos;

public class SeatUpdateDto : ISeatDto
{
    public int Id { get; set; }
    public string SeatNumber { get; set; }
    public bool IsAvailable { get; set; }
}

