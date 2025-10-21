using System.Text.Json.Serialization;

namespace FlightsReservation.BLL.Entities.Utilities.Requests;

public class PayuOrderRequest
{
    public string Description { get; set; } = null!;

    [JsonIgnore]
    public string CurrencyCode { get; set; } = "PLN";
    public int TotalAmount { get; set; }

    [JsonIgnore]
    public string CustomerIp { get; set; } = "127.0.0.1";
    public string BuyerEmail { get; set; } = "test@example.com";
}