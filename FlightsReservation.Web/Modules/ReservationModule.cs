using FlightsReservation.BLL.DtoEntities.ReservationDtos;
using FlightsReservation.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Carter;
using FlightsReservation.Web.Extensions;

namespace FlightsReservation.Web.Modules;

public class ReservationModule() : CarterModule("/Reservations")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetReservation", async (Guid id, ReservationsService service, CancellationToken ct = default) =>
        {
            var response = await service.GetReservationByIdAsync(id, ct);
            return response.ToHttpResult();
        });

        app.MapPost("/AddReservation",
            async ([FromBody] ReservationCreateDto createDto, ReservationsService service, CancellationToken ct = default) =>
            {
                var response = await service.AddReservationAsync(createDto, ct);
                return response.ToHttpResult();
            });

        app.MapDelete("/DeleteReservation", async (Guid id, ReservationsService service, CancellationToken ct = default) =>
        {
            var response = await service.DeleteReservation(id, ct);
            return response.ToHttpResult();
        });
    }
}