using FlightsReservation.BLL.DtoEntities.PassengerDtos;
using FlightsReservation.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Carter;

namespace FlightsReservation.Web.Modules;

public class PassengerModule() : CarterModule("/Passengers")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetPassenger", async (Guid id, PassengersService service, CancellationToken ct = default) =>
        {
            var response = await service.GetPassengerByIdAsync(id, ct);
            return Results.Ok(response);
        });

        app.MapPut("/UpdatePassenger",
            async ([FromBody] PassengerUpdateDto productUpdateDto, PassengersService service, CancellationToken ct = default) =>
            {
                await service.UpdatePassengerAsync(productUpdateDto, ct);
                return Results.Ok();
            });

        app.MapDelete("/DeletePassenger", async (Guid id, PassengersService service, CancellationToken ct = default) =>
        {
            await service.DeletePassengerAsync(id, ct);
            return Results.Ok();
        });
    }
}