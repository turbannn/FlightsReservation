using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Carter;
using FlightsReservation.BLL.Entities.DataTransferObjects.ReservationDtos;
using FlightsReservation.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using FlightsReservation.BLL.Services.EntityServices;

namespace FlightsReservation.Web.Modules;

public class ReservationModule() : CarterModule("/Reservations")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetReservation", async (Guid id, ReservationsService service, CancellationToken ct = default) =>
        {
            var response = await service.GetReservationByIdAsync(id, ct);
            return response.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        app.MapPost("/CommitReservation",
            async ([FromBody] ReservationCreateDto createDto, HttpContext http, ReservationsService service, CancellationToken ct = default) =>
            {
                var idStr = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(idStr) || !Guid.TryParse(idStr, out Guid userId))
                    return Results.BadRequest(new { success = false, message = "Invalid user ID" });

                var response = await service.CommitReservationAsync(userId, createDto, ct);
                return response.ToHttpResult();
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "User" });

        app.MapDelete("/DeleteReservation", async (Guid id, ReservationsService service, CancellationToken ct = default) =>
        {
            var response = await service.DeleteReservation(id, ct);
            return response.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        app.MapPost("/BeginReservation", async (List<Guid> ids, SeatsService service, CancellationToken ct = default) =>
        {
            var response = await service.LockSeats(ids, ct);
            return response.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "User" });
    }
}