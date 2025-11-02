using FlightsReservation.BLL.Entities.Utilities.Requests;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.Entities.Utilities.Results;
using FlightsReservation.DAL.Entities.Utils.Payment;
using FlightsReservation.DAL.Interfaces;
using FlightsReservation.DAL.UoWs;
using FluentValidation;
using FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;

namespace FlightsReservation.BLL.Services.UtilityServices.Payment;

public class PayuService
{
    private readonly PayuSettings _payuSettings;
    private readonly IValidator<PayuOrderRequest> _payuOrderRequestValidator;
    private readonly HttpClient _httpClient;

    public PayuService(PayuSettings payuSettings, IValidator<PayuOrderRequest> validator, IHttpClientFactory httpClientFactory)
    {
        _payuSettings = payuSettings;
        _payuOrderRequestValidator = validator;
        _httpClient = httpClientFactory.CreateClient("PayU");
    }

    private async Task<string> GetTokenAsync(CancellationToken ct)
    {
        var url = "/pl/standard/user/oauth/authorize";

        _httpClient.BaseAddress = new Uri(_httpClient.BaseAddress+url);

        var form = new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _payuSettings.ClientId),
                new KeyValuePair<string, string>("client_secret", _payuSettings.ClientSecret)
            });

        var res = await _httpClient.PostAsync(url, form, cancellationToken: ct);

        if (!res.IsSuccessStatusCode)
        {
            var err = await res.Content.ReadAsStringAsync(ct);
            throw new Exception($"GetTokenAsync failed: {res.StatusCode}. Body: {err}");
        }

        var json = await res.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("access_token").GetString()!;
    }


    public async Task<FlightReservationResult<PayuOrderResult>> CreateOrderAsync(PayuOrderRequest request, CancellationToken ct)
    {
        var validationResult = await _payuOrderRequestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First();

            return FlightReservationResult<PayuOrderResult>.Fail(error.ToString(), ResponseCodes.BadRequest);
        }

        var handler = new HttpClientHandler { AllowAutoRedirect = false };
        using var client = new HttpClient(handler);
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetTokenAsync(ct));

        var body = new
        {
            notifyUrl = _payuSettings.NotifyUrl,
            continueUrl = $"{_payuSettings.ContinueUrl}?amount={request.TotalAmount}",
            customerIp = request.CustomerIp,
            merchantPosId = _payuSettings.PosId,
            description = request.Description,
            currencyCode = request.CurrencyCode,
            totalAmount = request.TotalAmount.ToString(),
            buyer = new { email = request.BuyerEmail },
            products = new[]
            {
                new { name = request.Description, unitPrice = request.TotalAmount.ToString(), quantity = 1 }
            }
        };

        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var res = await client.PostAsync($"{_payuSettings.BaseUrl}/api/v2_1/orders", content, cancellationToken: ct);

        if (res.StatusCode != System.Net.HttpStatusCode.Found || res.StatusCode == System.Net.HttpStatusCode.Redirect)
        {
            if (res.Headers.Location != null)
            {
                return FlightReservationResult<PayuOrderResult>.Success(new PayuOrderResult()
                {
                    Status = "SUCCESS",
                    RedirectUri = res.Headers.Location.ToString(),
                    OrderId = ""
                }, ResponseCodes.Success);
            }
        }

        var responseJson = await res.Content.ReadAsStringAsync(ct);
        if (!res.IsSuccessStatusCode)
            return FlightReservationResult<PayuOrderResult>.Fail($"CreateOrderAsync failed: {res.StatusCode}. Body: {responseJson}", ResponseCodes.InternalServerError);

        using var doc = JsonDocument.Parse(responseJson);

        return FlightReservationResult<PayuOrderResult>.Success(new PayuOrderResult()
        {
            Status = doc.RootElement.GetProperty("status").GetProperty("statusCode").GetString()!,
            RedirectUri = doc.RootElement.GetProperty("redirectUri").GetString()!,
            OrderId = doc.RootElement.GetProperty("orderId").GetString()!
        }, ResponseCodes.Success);
    }
}