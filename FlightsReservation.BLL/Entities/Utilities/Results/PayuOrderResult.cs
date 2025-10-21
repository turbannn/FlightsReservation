
namespace FlightsReservation.BLL.Entities.Utilities.Results;

public class PayuOrderResult
{
    public string Status { get; set; } = null!;
    public string RedirectUri { get; set; } = null!;
    public string OrderId { get; set; } = null!;
}