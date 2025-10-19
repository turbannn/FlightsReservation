using System.Text.Json.Serialization;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.UserDtos;

public class UserCreateDto : BaseUserDto
{
    [JsonIgnore]
    public string Role { get; set; } = null!;

    [JsonIgnore]
    public int Money { get; set; }
}