namespace FlightsReservation.BLL.Entities.DataTransferObjects.UserDtos;

public class UserUpdateDto : BaseUserDto
{
    public Guid Id { get; set; }
    public double Money { get; set; }
}