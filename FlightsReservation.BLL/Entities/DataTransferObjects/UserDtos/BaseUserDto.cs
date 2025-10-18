using FlightsReservation.BLL.Interfaces.Dtos;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.UserDtos;

public class BaseUserDto : IUserDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int Money { get; set; }
}