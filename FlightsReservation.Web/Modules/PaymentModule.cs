using Carter;
using FlightsReservation.BLL.Entities.Utilities.Requests;
using FlightsReservation.BLL.Services.EntityServices;
using FlightsReservation.BLL.Services.UtilityServices.Payment;
using FlightsReservation.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });

        app.MapGet("/CommitPayment", async ([FromQuery] double amount, HttpContext http, UsersService service, CancellationToken ct = default) =>
        {
            var idStr = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idStr) || !Guid.TryParse(idStr, out Guid userId))
                return Results.BadRequest(new { success = false, message = "Invalid user ID" });

            var response = await service.AddUserMoneyAsync(userId, amount, ct);

            await service.AddUserMoneyAsync(userId, amount, ct);

            if (!response.IsSuccess)
                return response.ToHttpResult();
            
            return Results.Redirect(response.Value!);
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });
    }
}