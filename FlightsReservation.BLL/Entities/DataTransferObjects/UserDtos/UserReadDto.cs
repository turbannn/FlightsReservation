namespace FlightsReservation.BLL.Entities.DataTransferObjects.UserDtos;

public class UserReadDto : BaseUserDto
{
    public Guid Id { get; set; }
    public string Role { get; set; } = null!;
    public int Money { get; set; }
}