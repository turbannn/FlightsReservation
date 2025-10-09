using FlightsReservation.BLL.DtoEntities.ReservationDtos;
using FlightsReservation.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Carter;

namespace FlightsReservation.Web.Modules;

public class ReservationModule() : CarterModule("/Reservations")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetReservation", async (Guid id, ReservationsService service, CancellationToken ct = default) =>
        {
            var response = await service.GetReservationByIdAsync(id, ct);
            return Results.Ok(response);
        });

        app.MapPost("/AddReservation",
            async ([FromBody] ReservationCreateDto createDto, ReservationsService service, CancellationToken ct = default) =>
            {
                await service.AddReservationAsync(createDto, ct);
                return Results.Ok();
            });

        app.MapDelete("/DeleteReservation", async (Guid id, ReservationsService service, CancellationToken ct = default) =>
        {
            await service.DeleteReservation(id, ct);
            return Results.Ok();
        });
    }
}