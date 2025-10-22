using Carter;
using FlightsReservation.BLL.Entities.Utilities.Requests;
using FlightsReservation.BLL.Services.EntityServices;
using FlightsReservation.BLL.Services.UtilityServices.Payment;
using FlightsReservation.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FlightsReservation.Web.Modules;

public class PaymentModule() : CarterModule("/Payments")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/CreatePayment", async (PayuOrderRequest request, PayuService service, CancellationToken ct = default) =>
        {
            request.CurrencyCode = "PLN";
            request.CustomerIp = "127.0.0.1";
            request.Description = "Flights Reservation payment order.";

            var response = await service.CreateOrderAsync(request, ct);
            return response.ToHttpResult();
        });

        app.MapGet("/CommitPayment", async ([FromQuery] int amount, UsersService service, CancellationToken ct = default) =>
        {
            var userId = Guid.Parse("3d781d10-d652-464c-82de-9b7d667549bf");

            var response = await service.AddUserMoneyAsync(userId, amount, ct);
            return response.ToHttpResult();
        });
    }
}