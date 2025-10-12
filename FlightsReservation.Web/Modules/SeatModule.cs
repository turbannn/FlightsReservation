using Microsoft.AspNetCore.Mvc;
using FlightsReservation.BLL.Services;
using Carter;
using FlightsReservation.BLL.Entities.DataTransferObjects.SeatDtos;

namespace FlightsReservation.Web.Modules;

public class SeatModule() : CarterModule("/Seats")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapGet("/GetSeat", async (Guid id, SeatsService service, CancellationToken ct = default) =>
        {
            var response = await service.GetSeatByIdAsync(id, ct);
            return Results.Ok(response);
        });

        app.MapPost("/AddSeat",
            async ([FromBody] SeatCreateDto createDto, SeatsService service, CancellationToken ct = default) =>
            {
                await service.AddSeatAsync(createDto, ct);
                return Results.Ok();
            });

        app.MapPut("/UpdateSeat",
            async ([FromBody] SeatUpdateDto productUpdateDto, SeatsService service, CancellationToken ct = default) =>
            {
                await service.UpdateSeatAsync(productUpdateDto, ct);
                return Results.Ok();
            });

        app.MapDelete("/DeleteSeat", async (Guid id, SeatsService service, CancellationToken ct = default) =>
        {
            await service.DeleteSeat(id, ct);
            return Results.Ok();
        });
    }
}