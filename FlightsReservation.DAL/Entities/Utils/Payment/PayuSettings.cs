
namespace FlightsReservation.DAL.Entities.Utils.Payment;

public class PayuSettings
{
    public string BaseUrl { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string PosId { get; set; } = null!;
    public string NotifyUrl { get; set; } = null!;
    public string ContinueUrl { get; set; } = null!;
}